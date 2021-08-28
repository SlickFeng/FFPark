using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
using FFPark.Data;
namespace FFPark.Services
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : BaseEntity
    {
        private readonly IRepository<TEntity> _services;
        private readonly IFreeSql _freeSql;

        public BaseServices(IRepository<TEntity> services, IFreeSql freeSql)
        {
            _services = services;
            _freeSql = freeSql;
        }
        public async Task<bool> Insert(TEntity entity)
        {
            var result = await _services.InsertAsync(entity);
            return result;
        }
        public async Task<TEntity> FindByIdAsync(int id)
        {
            var result = await _services.FindByIdAsync(id);
            return result;
        }

        public async Task<bool> Update(TEntity entity)
        {
            var result = await _services.UpdateAsync(entity);
            return result;
        }

        public async Task<bool> Update(Expression<Func<TEntity, TEntity>> exp, Expression<Func<TEntity, bool>> whereColoumExpression)
        {
            var result = await _services.UpdateAsync(exp, whereColoumExpression);
            return result;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> exp)
        {
            var result = await _services.CountAsync(exp);
            return result;
        }

    }
}
