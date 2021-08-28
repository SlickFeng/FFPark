using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;

namespace FFPark.Services
{
    public interface IBaseServices<TEntity> where TEntity : BaseEntity
    {
        Task<bool> Insert(TEntity entity);

        Task<bool> Update(TEntity entity);

        Task<bool> Update(Expression<Func<TEntity, TEntity>> exp, Expression<Func<TEntity, bool>> whereColoumExpression);

        Task<TEntity> FindByIdAsync(int id);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> exp);
    }
}
