using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFPark.Core;
using System.Linq;
using FFPark.Core.Caching;
using System.Linq.Expressions;
using FFPark.Model;
using FFPark.Core.Extensions;
namespace FFPark.Data
{
    public partial class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {

        #region    Fileds
        private readonly IFreeSql _freeSql;
        #endregion


        public EntityRepository(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }


        public Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据查询条件,查询实体列表
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> exp)
        {
            var result = _freeSql.Select<TEntity>().Where(exp).ToListAsync();
            return result;
        }

        //public Task<TReturn> GetAsyncWithColumn<TReturn>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn>> select)
        //{
        //    var result = _freeSql.Select<TEntity>().Where(exp).ToOneAsync<TReturn>(select);
        //    return result;
        //}

        public Task<List<TReturn>> GetAsyncWithColumn<TReturn>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn>> select)
        {
            var result = _freeSql.Select<TEntity>().Where(exp).ToListAsync<TReturn>(select);
            return result;
        }


        public Task<TReturn> GetOneAsyncWithColumn<TReturn>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn>> select)
        {
            var result = _freeSql.Select<TEntity>().Where(exp).ToOneAsync<TReturn>(select);
            return result;
        }
        /// <summary>
        /// 查询列表指定列
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="exp">查询条件</param>
        /// <param name="orderbyColumn">排序字段,倒叙</param>
        /// <param name="select">查询字段</param>
        /// <returns></returns>
        public Task<List<TReturn>> GetAsyncWithColumn<TReturn>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, object>> orderbyColumn, Expression<Func<TEntity, TReturn>> select)
        {
            var result = _freeSql.Select<TEntity>().Where(exp).OrderByDescending(orderbyColumn).ToListAsync<TReturn>(select);
            return result;
        }

        public Task<TEntity> FindByIdAsync(int? id)
        {
            var result = _freeSql.Select<TEntity>().Where(t => t.Id == id).FirstAsync();
            return result;
        }


        /// <summary>
        /// 根据查询条件 查询实体列表
        /// </summary>
        /// <param name="exp">查询条件</param>
        /// <param name="column">排序字段-倒序</param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, object>> column)
        {
            var result = await _freeSql.Select<TEntity>().Where(exp).OrderByDescending(column).ToListAsync();
            return result;
        }
        /// <summary>
        /// 根据条件查询一条实体记录
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> exp)
        {
            var result = await _freeSql.Select<TEntity>().Where(exp).ToOneAsync();
            return result;
        }
        /// <summary>
        /// 判断是否存在列为XX 的数据
        /// </summary>
        /// <param name="exp"></param>
        /// <returns>是否存在数据</returns> 
        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> exp)
        {
            var resp = await _freeSql.Select<TEntity>().AnyAsync(exp);
            return resp;
        }
        /// <summary>
        /// 根据条件查询数据总数
        /// </summary>
        /// <param name="exp">查询条件</param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> exp)
        {
            var result = await _freeSql.Select<TEntity>().Where(exp).CountAsync();
            int resultint = 0;
            Int32.TryParse(result.ToString(), out resultint);
            return resultint;
        }
        /// <summary>
        /// 根据条件获取当前实体分页集合 
        /// </summary>
        /// <typeparam name="TEntity">当前实体</typeparam> 
        /// <param name="whereExpression">条件</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="column">排序属性</param>
        /// <param name="descending">是否倒叙</param>
        public async Task<PageModel<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> whereExpression = null, int pageIndex = 1, int pageSize = 20, Expression<Func<TEntity, object>> column = null, bool descending = false)
        {
            var result = new PageModel<TEntity>();
            var list = _freeSql.Select<TEntity>()
                 .WhereIf(whereExpression != null, whereExpression)
                 .OrderByIf(column != null, column, descending)
                 .Count(out var total) //总记录数量
                 .Page(pageIndex, pageSize)
                 .ToListAsync();
            result.Data = await list;
            result.DataCount = Convert.ToInt32(total);
            result.PageSize = pageSize;
            result.PageCount = (Math.Ceiling(total.ObjToDecimal() / pageSize.ObjToDecimal())).ObjToInt();
            return result;

        }

        /// <summary>
        /// 根据条件获取当前实体Dto分页集合 
        /// </summary>
        /// <typeparam name="TEntity">当前实体</typeparam> 
        /// <param name="whereExpression">条件</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="column">排序属性</param>
        /// <param name="descending">是否倒叙</param>
        public async Task<PageModel<TDto>> GetPageDtoAsync<TDto>(Expression<Func<TEntity, bool>> whereExpression = null, int pageIndex = 1, int pageSize = 20,
              Expression<Func<TEntity, object>> column = null, bool descending = false)
        {
            var result = new PageModel<TDto>();
            var list = _freeSql.Select<TEntity>()
                 .WhereIf(whereExpression != null, whereExpression)
                 .OrderByIf(column != null, column, descending)
                 .Count(out var total) //总记录数量
                 .Page(pageIndex, pageSize)
                 .ToListAsync<TDto>();
            result.Data = await list;
            result.DataCount = Convert.ToInt32(total);
            result.PageSize = pageSize;
            result.PageCount = (Math.Ceiling(total.ObjToDecimal() / pageSize.ObjToDecimal())).ObjToInt();
            return result;
        }
        /// <summary>
        /// 根据id 查询单表数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="getCacheKey"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByIdAsync(int? id, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            var result = await _freeSql.Select<TEntity>().Where(t => t.Id == id).ToOneAsync();
            return result;
        }
        /// <summary>
        /// 根据Id 集合查询单表数据集
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="getCacheKey"></param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            var result = await _freeSql.Select<TEntity>().Where(t => ids.Contains(t.Id)).ToListAsync();
            return result;
        }


        #region  新增

        public async Task<bool> InsertAsync(TEntity entity, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var result = await _freeSql.Insert(entity).ExecuteAffrowsAsync();
            return result > 0 ? true : false;
        }

        public async Task<int> InsertAsync(IList<TEntity> entities, bool publishEvent = true)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            IEnumerable<TEntity> iTentity = entities.AsEnumerable();
            return await _freeSql.Insert(iTentity).ExecuteAffrowsAsync();
        }

        public async Task<int> InsertAsyncIdentity(TEntity entity, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            return (int)await _freeSql.Insert(entity).ExecuteIdentityAsync();
        }


        #endregion

        #region  修改
        /// <summary>
        /// 整个实体修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        public async Task<bool> UpdateAsync(TEntity entity, bool publishEvent = true)
        {
            var result = await _freeSql.Update<TEntity>().SetSource(entity).ExecuteAffrowsAsync();

            return result > 0 ? true : false;

        }

        /// <summary>
        /// 指定列更新
        /// </summary>
        /// <param name="whereColoumExpression"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Expression<Func<TEntity, TEntity>> exp, Expression<Func<TEntity, bool>> whereColoumExpression)
        {
            return await _freeSql.Update<TEntity>()
                .Set(exp)
                .Where(whereColoumExpression).ExecuteAffrowsAsync() > 0 ? true : false;
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="publishEvent"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(IList<TEntity> entities, bool publishEvent = true)
        {
            var result = await _freeSql.Update<TEntity>().SetSource(entities).ExecuteAffrowsAsync();
            return result;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="">删除条件</param>

        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> exp)
        {
            var result = await _freeSql.Delete<TEntity>().Where(exp).ExecuteAffrowsAsync();
            return result > 0 ? true : false;
        }

        #endregion
    }
}
