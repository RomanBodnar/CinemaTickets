using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Abstract
{

    public interface IRepository<TEntity> where TEntity : class
    {
        ICollection<TEntity> GetAll();
        TEntity GetById(int? id);
        TEntity Find(Expression<Func<TEntity, bool>> match);
        void Edit(TEntity entity, int id);
        void Insert(TEntity entity);
        Task<ICollection<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int? id);
       
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match);
       
        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task EditAsync(TEntity entity, int id);

        Task InsertAsync(TEntity entity);
      
        Task<int> DeleteAsync(TEntity entity);
        int Delete(TEntity entity);
    }
}
