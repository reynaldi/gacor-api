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

        Task<Tuple<UserDto, ErrorBusiness>> GetUserProfile(string email);
        Task<ErrorBusiness> EditUser(UserDto data);
    }
}