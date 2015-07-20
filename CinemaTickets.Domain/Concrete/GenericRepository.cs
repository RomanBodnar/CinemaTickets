using CinemaTickets.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Threading;

namespace CinemaTickets.Domain.Concrete
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected EFContext context;// = new EFContext();
        protected SemaphoreSlim mutex = new SemaphoreSlim(1);
        public GenericRepository()
        {
            context = new EFContext();
        }
        public GenericRepository(EFContext context)
        {
            this.context = context;
        }

        public ICollection<TEntity> GetAll()
        {

            try
            {
                return context.Set<TEntity>().ToList();
            }
            catch (SqlException e)
            {

                throw e;
            }
        }

        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                return await context.Set<TEntity>().ToListAsync();
            }
            catch (SqlException e)
            {

                throw e;
            }
            finally
            {
                mutex.Release();
            }
        }
        public async Task<TEntity> GetByIdAsync(int? id)
        {
            await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                TEntity result = await context.Set<TEntity>().FindAsync(id);
                return result;
            }
            catch (SqlException e)
            {

                throw e;
            }
            finally
            {
                mutex.Release();
            }
        }
        public TEntity GetById(int? id)
        {
            try
            {
                TEntity result = context.Set<TEntity>().Find(id);
                return result;
            }
            catch (SqlException e)
            {

                throw e;
            }
        }
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                TEntity result = await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
          //      ((IObjectContextAdapter)context).ObjectContext.Detach(result);
           //     context.Entry(result).State = EntityState.Detached;
               
                return result;
            }
            catch (SqlException e)
            {

                throw e;
            }
            finally
            {
                mutex.Release();
            }
        }
        public TEntity Find(Expression<Func<TEntity, bool>> match)
        {
            try
            {
                TEntity result = context.Set<TEntity>().FirstOrDefault(match);
                //      ((IObjectContextAdapter)context).ObjectContext.Detach(result);
                //     context.Entry(result).State = EntityState.Detached;

                return result;
            }
            catch (SqlException e)
            {

                throw e;
            }
        }
        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                ICollection<TEntity> results = await context.Set<TEntity>().Where(predicate).ToListAsync();
                return results;
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                mutex.Release();
            }
        }
        public async Task EditAsync(TEntity entity, int id)
        {
            await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                if (entity == null)
                {
                    await InsertAsync(entity);
                    return;
                }

                TEntity existing = await context.Set<TEntity>().FindAsync(id);
                if (existing != null)
                {
                    context.Entry(existing).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
                }

            }
            catch (SqlException e)
            {

                throw e;
            }
            finally
            {
                mutex.Release();
            }
        }
        public void Edit(TEntity entity, int id)
        {
            try
            {
                if (entity == null)
                {
                    Insert(entity);
                    return;
                }

                TEntity existing = context.Set<TEntity>().Find(id);
                if (existing != null)
                {
                    context.Entry(existing).CurrentValues.SetValues(entity);
                    context.SaveChanges();
                }

            }
            catch (SqlException e)
            {

                throw e;
            }
        }
        public async Task InsertAsync(TEntity entity)
        {
            await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                context.Set<TEntity>().Add(entity);
                await context.SaveChangesAsync();
            }
            catch (SqlException e)
            {

                throw e;
            }
            finally
            {
                mutex.Release();
            }
        }
        public void Insert(TEntity entity)
        {
            try
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            }
            catch (SqlException e)
            {

                throw e;
            }
        }
        public async Task<int> DeleteAsync(TEntity t)
        {
            await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                context.Set<TEntity>().Remove(t);
                return await context.SaveChangesAsync();
            }
            catch (SqlException e)
            {

                throw e;
            }
            finally
            {
                mutex.Release();
            }
        }
        public int Delete(TEntity t)
        {
            
            try
            {
                context.Set<TEntity>().Remove(t);
                return context.SaveChanges();
            }
            catch (SqlException e)
            {

                throw e;
            }
            
        }
    }
}
