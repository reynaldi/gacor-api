using System.Linq.Expressions;

namespace GacorAPI.Data.Repositories
{
    public interface IGeneralRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null,
            string include = ""
            );
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null,
            string include = ""
            );
        TEntity GetOne(Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null);
        TEntity GetById(long id);
        Task<TEntity> GetByIdAsync(long id);
        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);
        void InsertAll(IEnumerable<TEntity> entities);
        Task InsertAllAsync(IEnumerable<TEntity> entities);
        void Delete(long id);
        Task DeleteAsync(long id);
        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Update(TEntity entityToUpdate);
        Task UpdateAsync(TEntity entityToUpdate);
    }
}