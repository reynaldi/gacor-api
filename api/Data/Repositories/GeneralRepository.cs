using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GacorAPI.Data.Repositories
{
    public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly GacorContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public GeneralRepository(GacorContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public void Delete(long id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            if(_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public async Task DeleteAsync(long id)
        {
            await Task.Run(() => {
                Delete(id);
            });
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => {
                Delete(entity);
            });
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null, 
            string include = ""
            )
        {
            IQueryable<TEntity> query = _dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }

            foreach(var includerProp in include.Split(new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includerProp);
            }

            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null, 
            string include = ""
            )
        {
            return await Task.Run(() => 
            {
                return Get(filter, orderBy, include);
            });
        }

        public TEntity GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void InsertAll(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task InsertAllAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            await Task.Run(() => 
            {
                Update(entityToUpdate);
            });
        }
    }
}