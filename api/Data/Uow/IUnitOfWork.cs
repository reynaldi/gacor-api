using GacorAPI.Data.Entities;
using GacorAPI.Data.Repositories;

namespace GacorAPI.Data.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        IGeneralRepository<User> UserRepository();
        IGeneralRepository<Blog> BlogRepository();
        IGeneralRepository<Comment> CommentRepository();
        void Save();
        Task SaveAsync();
    }
}