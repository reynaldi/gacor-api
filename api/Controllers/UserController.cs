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

        [HttpGet]
        public async Task<ActionResult<ResponseBody>> Get([FromQuery] string email)
        {
            var result = await _userService.GetUserProfile(email);
            return Ok(ResponseBodyGenerator.Generate(result.Item1, result.Item2));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseBody>> Put(long id, [FromBody] UserDto data)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.EditUser(data);
            if(result != null) return Ok(ResponseBodyGenerator.Generate(null, result));
            return NoContent();
        }
    }
}