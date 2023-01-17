using GacorAPI.Domain.UserDom.Dto;

namespace GacorAPI.Domain.UserDom
{
    public interface IUserService
    {
        /// <summary>
        /// Create user with password
        /// </summary>
        /// <param name="data">User data</param>
        /// <returns></returns>
        Task CreateUser(UserDto data);
    }
}