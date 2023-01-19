using GacorAPI.Controllers;
using GacorAPI.Domain.UserDom;
using GacorAPI.Domain.UserDom.Dto;
using GacorAPI.Infra.Body;
using GacorAPI.Infra.Errors;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GacorApi.Tests.ControllerDom
{
    public class UserControllerTest
    {
        [Fact]
        public async Task Get_Valid()
        {
            // arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(_ => _.GetUserProfile(It.IsAny<string>())).ReturnsAsync(new Tuple<UserDto, ErrorBusiness>(new UserDto
            {
                Email = "reynaldi.surya@gmail.com",
                FirstName = "Reynaldi",
                LastName = "Pranata Surya",
                Id = 1,
            },
            null as ErrorBusiness));
            var controller = new UserController(mockService.Object);

            // act
            var result = await controller.Get("reynaldi.surya@gmail.com");

            // assert
            var act = result.Result as OkObjectResult;
            Assert.NotNull(act);
            Assert.NotNull(act.Value);
            var model = (ResponseBody)act.Value;
            Assert.NotNull(model);
            var assess = model.Result as UserDto;
            Assert.NotNull(assess);
            Assert.Equal("reynaldi.surya@gmail.com", assess.Email);
        }
    }
}