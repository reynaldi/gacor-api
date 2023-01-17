using GacorAPI.Data.Entities;
using GacorAPI.Data.Uow;
using GacorAPI.Domain.UserDom.Dto;
using GacorAPI.Infra;

namespace GacorAPI.Domain.UserDom
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task CreateUser(UserDto data)
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
        }
    }
}