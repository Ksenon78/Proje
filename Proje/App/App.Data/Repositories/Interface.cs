using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {


        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);

    }
}
