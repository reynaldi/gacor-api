using GacorAPI.Domain.UserDom;
using GacorAPI.Domain.UserDom.Dto;
using GacorAPI.Infra.Body;
using GacorAPI.Infra.Errors;
using Microsoft.AspNetCore.Mvc;

namespace GacorAPI.Controllers
{
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseBody>> Post([FromBody] UserDto data)
        {
            var err = await _userService.CreateUser(data);
            if(err != null)
            {
                return Ok(ErrorBodyGenerator.Generate(err));
            }
            return Ok();
        }
    }
}