﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FFPark.Core.ComponentModel;
using FFPark.Core.Configuration;
using FFPark.Core.Redis;
using FFPark.Core.Security;

namespace FFPark.Core.Caching
{
    public partial class RedisCacheManager : CacheKeyService, IStaticCacheManager
    {
        #region Fields

        private bool _disposed;
        private IDatabase _db;
        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly PerRequestCache _perRequestCache;

        #endregion

        #region Ctor

        public RedisCacheManager(AppSettings appSettings,
            IHttpContextAccessor httpContextAccessor,
            IRedisConnectionWrapper connectionWrapper) : base(appSettings)
        {
            if (string.IsNullOrEmpty(appSettings.RedisConfig.ConnectionString))
                throw new Exception("Redis connection string is empty");

            // ConnectionMultiplexer.Connect should only be called once and shared between callers
            _connectionWrapper = connectionWrapper;

            _perRequestCache = new PerRequestCache(httpContextAccessor);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get the list of cache keys prefix
        /// </summary>
        /// <param name="endPoint">Network address</param>
        /// <param name="prefix">String key pattern</param>
        /// <returns>List of cache keys</returns>
        protected virtual async Task<IEnumerable<RedisKey>> GetKeysAsync(EndPoint endPoint, string prefix = null)
        {
            var server = await _connectionWrapper.GetServerAsync(endPoint);

            //we can use the code below (commented), but it requires administration permission - ",allowAdmin=true"
            //server.FlushDatabase();

            var keys = server.Keys((await GetDatabaseAsync()).Database, string.IsNullOrEmpty(prefix) ? null : $"{prefix}*");

            //we should always persist the data protection key list
            keys = keys.Where(key => !key.ToString().Equals(FFParkDataProtectionDefaults.RedisDataProtectionKey,
                StringComparison.OrdinalIgnoreCase));

            return keys;
        }

        /// <summary>
        /// Get the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <returns>The cached value associated with the specified key</returns>
        protected virtual async Task<T> GetAsync<T>(CacheKey key)
        {
            //little performance workaround here:
            //we use "PerRequestCache" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server many times per HTTP request (e.g. each time to load a locale or setting)
            if (_perRequestCache.IsSet(key.Key))
                return _perRequestCache.Get(key.Key, () => default(T));

            //get serialized item from cache
            var serializedItem = await (await GetDatabaseAsync()).StringGetAsync(key.Key);
            if (!serializedItem.HasValue)
                return default;

            //deserialize item
            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default;

            //set item in the per-request cache
            _perRequestCache.Set(key.Key, item);

            return item;
        }

        /// <summary>
        /// Add the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        /// <param name="cacheTime">Cache time in minutes</param>
        protected virtual async Task SetAsync(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            //set cache time
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);

            //and set it to cache
            await (await GetDatabaseAsync()).StringSetAsync(key, serializedItem, expiresIn);
        }

        /// <summary>
        /// Try to execute the passed function and ignore the RedisTimeoutException if specified by settings
        /// </summary>
        /// <typeparam name="T">Type of item which returned by the action</typeparam>
        /// <param name="action">The function to be tried to perform</param>
        /// <returns>(flag indicates whether the action was executed without error, action result or default value)</returns>
        protected virtual async Task<(bool, T)> TryPerformActionAsync<T>(Func<Task<T>> action)
        {
            try
            {
                //attempts to execute the passed function
                var rez = await action();

                return (true, rez);
            }
            catch (RedisTimeoutException)
            {
                //ignore the RedisTimeoutException if specified by settings
                if (_appSettings.RedisConfig.IgnoreTimeoutException)
                    return (false, default);

                //or rethrow the exception
                throw;
            }
        }

        private async Task<IDatabase> GetDatabaseAsync()
        {
            if (_db != null)
                return _db;

            _db = await _connectionWrapper.GetDatabaseAsync(_appSettings.RedisConfig.DatabaseId ?? (int)RedisDatabaseNumber.Cache);

            return _db;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
        {
            //item already is in cache, so return it
            if (await IsSetAsync(key))
                return await GetAsync<T>(key);

            //or create it using passed function
            var result = await acquire();

            //and set in cache (if cache time is defined)
            if (key.CacheTime > 0)
                await SetAsync(key.Key, result, key.CacheTime);

            return result;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetAsync<T>(CacheKey key, Func<T> acquire)
        {
            //item already is in cache, so return it
            if (await IsSetAsync(key))
                return await GetAsync<T>(key);

            //or create it using passed function
            var result = acquire();

            //and set in cache (if cache time is defined)
            if (key.CacheTime > 0)
                await SetAsync(key.Key, result, key.CacheTime);

            return result;
        }

        /// <summary>
        /// Add the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        public virtual async Task SetAsync(CacheKey key, object data)
        {
            if (data == null)
                return;

            //set cache time
            var expiresIn = TimeSpan.FromMinutes(key.CacheTime);

            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);

            //and set it to cache
            await TryPerformActionAsync(async () => await (await GetDatabaseAsync()).StringSetAsync(key.Key, serializedItem, expiresIn));
            _perRequestCache.Set(key.Key, data);
        }

        /// <summary>
        /// Get a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        public virtual async Task<bool> IsSetAsync(CacheKey key)
        {
            //little performance workaround here:
            //we use "PerRequestCache" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server many times per HTTP request (e.g. each time to load a locale or setting)
            if (_perRequestCache.IsSet(key.Key))
                return true;

            var (flag, rez) = await TryPerformActionAsync(async () => await (await GetDatabaseAsync()).KeyExistsAsync(key.Key));

            return flag && rez;
        }

        /// <summary>
        /// Remove the value with the specified key from the cache
        /// </summary>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="cacheKeyParameters">Parameters to create cache key</param>
        public async Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            cacheKey = PrepareKey(cacheKey, cacheKeyParameters);

            //we should always persist the data protection key list
            if (cacheKey.Key.Equals(FFParkDataProtectionDefaults.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase))
                return;

            //remove item from caches
            await TryPerformActionAsync(async () => await (await GetDatabaseAsync()).KeyDeleteAsync(cacheKey.Key));
            _perRequestCache.Remove(cacheKey.Key);
        }

        /// <summary>
        /// Remove items by cache key prefix
        /// </summary>
        /// <param name="prefix">Cache key prefix</param>
        /// <param name="prefixParameters">Parameters to create cache key prefix</param>
        public async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
        {
            prefix = PrepareKeyPrefix(prefix, prefixParameters);

            _perRequestCache.RemoveByPrefix(prefix);

            foreach (var endPoint in await _connectionWrapper.GetEndPointsAsync())
            {
                var keys = await GetKeysAsync(endPoint, prefix);

                await TryPerformActionAsync(async () => await (await GetDatabaseAsync()).KeyDeleteAsync(keys.ToArray()));
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual async Task ClearAsync()
        {
            foreach (var endPoint in await _connectionWrapper.GetEndPointsAsync())
            {
                var keys = (await GetKeysAsync(endPoint)).ToArray();

                //we can't use _perRequestCache.Clear(),
                //because HttpContext stores some server data that we should not delete
                foreach (var redisKey in keys)
                    _perRequestCache.Remove(redisKey.ToString());

                await TryPerformActionAsync(async () => await (await GetDatabaseAsync()).KeyDeleteAsync(keys.ToArray()));
            }
        }

        /// <summary>
        /// Dispose cache manager
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;
        }

        #endregion

        #region Nested class

        /// <summary>
        /// Represents a manager for caching during an HTTP request (short term caching)
        /// </summary>
        protected class PerRequestCache
        {
            #region Fields

            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ReaderWriterLockSlim _locker;

            #endregion

            #region Ctor

            public PerRequestCache(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;

                _locker = new ReaderWriterLockSlim();
            }

            #endregion

            #region Utilities

            /// <summary>
            /// Get a key/value collection that can be used to share data within the scope of this request
            /// </summary>
            protected virtual IDictionary<object, object> GetItems()
            {
                return _httpContextAccessor.HttpContext?.Items;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Get a cached item. If it's not in the cache yet, then load and cache it
            /// </summary>
            /// <typeparam name="T">Type of cached item</typeparam>
            /// <param name="key">Cache key</param>
            /// <param name="acquire">Function to load item if it's not in the cache yet</param>
            /// <returns>The cached value associated with the specified key</returns>
            public virtual T Get<T>(string key, Func<T> acquire)
            {
                IDictionary<object, object> items;

                using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.Read))
                {
                    items = GetItems();
                    if (items == null)
                        return acquire();

                    //item already is in cache, so return it
                    if (items[key] != null)
                        return (T)items[key];
                }

                //or create it using passed function
                var result = acquire();

                //and set in cache (if cache time is defined)
                using (new ReaderWriteLockDisposable(_locker))
                    items[key] = result;

                return result;
            }

            /// <summary>
            /// Add the specified key and object to the cache
            /// </summary>
            /// <param name="key">Key of cached item</param>
            /// <param name="data">Value for caching</param>
            public virtual void Set(string key, object data)
            {
                if (data == null)
                    return;

                using (new ReaderWriteLockDisposable(_locker))
                {
                    var items = GetItems();
                    if (items == null)
                        return;

                    items[key] = data;
                }
            }

            /// <summary>
            /// Get a value indicating whether the value associated with the specified key is cached
            /// </summary>
            /// <param name="key">Key of cached item</param>
            /// <returns>True if item already is in cache; otherwise false</returns>
            public virtual bool IsSet(string key)
            {
                using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.Read))
                {
                    var items = GetItems();
                    return items?[key] != null;
                }
            }

            /// <summary>
            /// Remove the value with the specified key from the cache
            /// </summary>
            /// <param name="key">Key of cached item</param>
            public virtual void Remove(string key)
            {
                using (new ReaderWriteLockDisposable(_locker))
                {
                    var items = GetItems();
                    items?.Remove(key);
                }
            }

            /// <summary>
            /// Remove items by key prefix
            /// </summary>
            /// <param name="prefix">String key prefix</param>
            public virtual void RemoveByPrefix(string prefix)
            {
                using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.UpgradeableRead))
                {
                    var items = GetItems();
                    if (items == null)
                        return;

                    //get cache keys that matches pattern
                    var regex = new Regex(prefix,
                        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    var matchesKeys = items.Keys.Select(p => p.ToString()).Where(key => regex.IsMatch(key)).ToList();

                    if (!matchesKeys.Any())
                        return;

                    using (new ReaderWriteLockDisposable(_locker))
                    {
                        //remove matching values
                        foreach (var key in matchesKeys)
                        {
                            items.Remove(key);
                        }
                    }
                }
            }

            /// <summary>
            /// Clear all cache data
            /// </summary>
            public virtual void Clear()
            {
                using (new ReaderWriteLockDisposable(_locker))
                {
                    var items = GetItems();
                    items?.Clear();
                }
            }

            #endregion
        }

        #endregion
    }
}
