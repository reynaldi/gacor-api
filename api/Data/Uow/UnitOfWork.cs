using System.Transactions;
using GacorAPI.Data.Entities;
using GacorAPI.Data.Repositories;

namespace GacorAPI.Data.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GacorContext _context;
        private IGeneralRepository<Blog> _blogRepository;
        private IGeneralRepository<Comment> _commentRepository;
        private IGeneralRepository<User> _userRepository;
        private TransactionScope _transaction;
        private bool _disposed = false;

        public UnitOfWork(GacorContext context)
        {
            _context = context;
        }

        public IGeneralRepository<Blog> BlogRepository()
        {
            if(_blogRepository == null)
            {
                _blogRepository = new GeneralRepository<Blog>(_context);
            }
            return _blogRepository;
        }

        public IGeneralRepository<Comment> CommentRepository()
        {
            if(_commentRepository == null)
            {
                _commentRepository = new GeneralRepository<Comment>(_context);
            }
            return _commentRepository;
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IGeneralRepository<User> UserRepository()
        {
            if(_userRepository == null)
            {
                _userRepository = new GeneralRepository<User>(_context);
            }
            return _userRepository;
        }

        public void StartTransaction()
        {
            _transaction = new TransactionScope();
        }

        public void CompleteTransaction()
        {
            _transaction.Complete();                        
        }

        public void DisposeTransaction()
        {
            _transaction.Dispose();
        }
    }
}