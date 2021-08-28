using System;
using System.Net;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace FFPark.Core.Redis
{
    public interface IRedisConnectionWrapper : IDisposable
    {
        /// <summary>
        /// Obtain an interactive connection to a database inside Redis
        /// </summary>
        /// <param name="db">Database number</param>
        /// <returns>Redis cache database</returns>
        Task<IDatabase> GetDatabaseAsync(int db);

        /// <summary>
        /// Obtain an interactive connection to a database inside Redis
        /// </summary>
        /// <param name="db">Database number</param>
        /// <returns>Redis cache database</returns>
        IDatabase GetDatabase(int db);

        /// <summary>
        /// Obtain a configuration API for an individual server
        /// </summary>
        /// <param name="endPoint">The network endpoint</param>
        /// <returns>Redis server</returns>
        Task<IServer> GetServerAsync(EndPoint endPoint);

        /// <summary>
        /// Gets all endpoints defined on the server
        /// </summary>
        /// <returns>Array of endpoints</returns>
        Task<EndPoint[]> GetEndPointsAsync();

        //TODO: may be deleted
        /// <summary>
        /// Delete all the keys of the database
        /// </summary>
        /// <param name="db">Database number</param>
        Task FlushDatabaseAsync(RedisDatabaseNumber db);
    }
}
