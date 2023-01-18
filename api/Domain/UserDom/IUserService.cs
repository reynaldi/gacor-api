using GacorAPI.Domain.UserDom.Dto;
using GacorAPI.Infra.Errors;

namespace GacorAPI.Domain.UserDom
{
    public interface IUserService
    {
        /// <summary>
        /// Create user with password
        /// </summary>
        /// <param name="data">User data</param>
        /// <returns></returns>
        Task<ErrorBusiness> CreateUser(UserDto data);
    }
}