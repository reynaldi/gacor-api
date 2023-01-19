using GacorAPI.Data.Entities;
using GacorAPI.Data.Uow;
using GacorAPI.Domain.UserDom.Dto;
using GacorAPI.Infra;
using GacorAPI.Infra.Errors;

namespace GacorAPI.Domain.UserDom
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<ErrorBusiness> CreateUser(UserDto data)
        {
            // check if user already registered or not
            var check = await _uow.UserRepository().GetAsync(u => u.Email == data.Email);
            if(check.Any()) return ErrorGenerator.Generate(ErrorCode.UserAlreadyRegistered);

            _uow.StartTransaction();
            try
            {
                var user = new User
                {
                    Email = data.Email,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    Password = HashHelper.HashPassword(data.Password)
                };

                await _uow.UserRepository().InsertAsync(user);
                await _uow.SaveAsync();
                _uow.CompleteTransaction();
                _uow.DisposeTransaction();
                return null;
            }
            catch (Exception)
            {
                _uow.DisposeTransaction();
                throw;
            }
        }

        public async Task<ErrorBusiness> EditUser(UserDto data)
        {
            var user = await _uow.UserRepository().GetByIdAsync(data.Id);
            if(user == null) return ErrorGenerator.Generate(ErrorCode.UserNotFound);
            try
            {
                _uow.StartTransaction();
                user.FirstName = data.FirstName;
                user.LastName = data.LastName;
                await _uow.UserRepository().UpdateAsync(user);
                await _uow.SaveAsync();
                _uow.CompleteTransaction();
                _uow.DisposeTransaction();
                return null;
            }
            catch (System.Exception)
            {
                _uow.DisposeTransaction();
                throw;
            }
            
        }

        public async Task<Tuple<UserDto, ErrorBusiness>> GetUserProfile(string email)
        {
            var result = await _uow.UserRepository().GetOneAsync(e => email.ToLower() == e.Email.ToLower());
            if(result == null)
            {
                return new Tuple<UserDto, ErrorBusiness>(null, ErrorGenerator.Generate(ErrorCode.UserNotFound));
            }
            return new Tuple<UserDto, ErrorBusiness>(mapUser(result), null);
        }

        private UserDto mapUser(User user)
        {
            return new UserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
            };
        }
    }
}