using System;
using FFPark.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using FFPark.Core.Caching;
using System.Linq;
using System.Linq.Expressions;
using FFPark.Model;
namespace FFPark.Data
{
    public partial interface IRepository<TEntity> where TEntity : BaseEntity
    {

        Task<TEntity> GetByIdAsync(int? id, Func<IStaticCacheManager, CacheKey> getCacheKey = null);
        Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="publishEvent"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(TEntity entity, bool publishEvent = true);

        /// <summary>
        ///  批量新增数据
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="publishEvent"></param>
        /// <returns></returns>
        Task<int> InsertAsync(IList<TEntity> entities, bool publishEvent = true);

        /// <summary>
        /// 新增数据 
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="publishEvent"></param>
        /// <returns>自增主键</returns>
        Task<int> InsertAsyncIdentity(TEntity entity, bool publishEvent = true);

        #region  修改
        /// <summary>
        /// 整个实体修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        Task<bool> UpdateAsync(TEntity entity, bool publishEvent = true);

        /// <summary>
        /// 指定列更新
        /// </summary>
        /// <param name="whereColoumExpression"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Expression<Func<TEntity, TEntity>> exp, Expression<Func<TEntity, bool>> whereColoumExpression);


        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="publishEvent"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(IList<TEntity> entities, bool publishEvent = true);

        #endregion


        #region   查询

        /// <summary>
        /// 查询指定列
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="exp">查询条件</param>
        /// <param name="select">指定列</param>
        /// <returns></returns>

        //Task<TReturn> GetAsyncWithColumn<TReturn>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn>> select);

        /// <summary>
        /// 查询列表指定列
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="exp"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        Task<List<TReturn>> GetAsyncWithColumn<TReturn>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn>> select);



        Task<TReturn> GetOneAsyncWithColumn<TReturn>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TReturn>> select );

        /// <summary>
        /// 查询列表指定列
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="exp">查询条件</param>
        /// <param name="orderbyColumn">排序字段,倒叙</param>
        /// <param name="select">查询字段</param>
        /// <returns></returns>
        Task<List<TReturn>> GetAsyncWithColumn<TReturn>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, object>> orderbyColumn, Expression<Func<TEntity, TReturn>> select);


        Task<TEntity> FindByIdAsync(int? Id);

        /// <summary>
        /// 根据查询条件,查询实体列表
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 根据条件查询数据总数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> exp);




        /// <summary>
        /// 根据查询条件 查询实体列表
        /// </summary>
        /// <param name="exp">查询条件</param>
        /// <param name="column">排序字段-倒序</param>
        /// <returns></returns>
        public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, object>> column);

        /// <summary>
        /// 根据条件查询一条实体记录
        /// </summary>
        /// <param name="exp">查询条件 t=>t.Id=1</param>
        /// <returns></returns>
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> exp);


        /// <summary>
        /// 根据条件获取当前实体分页集合 
        /// </summary>
        /// <typeparam name="TEntity">当前实体</typeparam> 
        /// <param name="whereExpression">条件</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="column">排序属性</param>
        /// <param name="descending">是否倒叙</param>
        Task<PageModel<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> whereExpression = null, int pageIndex = 1, int pageSize = 20, Expression<Func<TEntity, object>> column = null, bool descending = false);

        /// <summary>
        /// 根据条件获取当前实体Dto分页集合 
        /// </summary>
        /// <typeparam name="TEntity">当前实体</typeparam> 
        /// <param name="whereExpression">条件</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="column">排序属性</param>
        /// <param name="descending">是否倒叙</param>
        Task<PageModel<TDto>> GetPageDtoAsync<TDto>(Expression<Func<TEntity, bool>> whereExpression = null, int pageIndex = 1, int pageSize = 20,
            Expression<Func<TEntity, object>> column = null, bool descending = false);

        /// <summary>
        /// 判断是否存在列为XX 的数据
        /// </summary>
        /// <param name="exp">是否存在的目标列</param>
        /// <returns></returns> 
        public Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> exp);


        #endregion


        #region   删除

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="">删除条件</param>

        /// <returns></returns>
        public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> exp);


        #endregion
    }
}
