using GacorAPI.Data.Entities;
using GacorAPI.Data.Repositories;
using GacorAPI.Data.Uow;
using GacorAPI.Domain.UserDom;
using GacorAPI.Domain.UserDom.Dto;
using GacorAPI.Infra;
using Moq;

namespace GacorApi.Tests.UserDom
{
    public class UserRepo
    {
        [Fact]
        public async Task InsertUser_Valid()
        {
            // arrange
            var mockRepo = new Mock<IGeneralRepository<User>>();
            mockRepo.Setup(x => x.InsertAsync(It.IsAny<User>()));

            var mock = new Mock<IUnitOfWork>();
            mock.Setup(s => s.UserRepository()).Returns(mockRepo.Object);

            // arrange user
            var userDto = new UserDto
            {
                FirstName = "reynaldi",
                LastName = "surya",
                Email = "reynaldi.surya@gmail.com",
                Password = "1234"
            };

            // act
            var userService = new UserService(mock.Object);
            await userService.CreateUser(userDto);

            // assert
            mockRepo.Verify(x => x.InsertAsync(It.Is<User>(arg => arg.Email == userDto.Email && arg.IsActive)), Times.Once);
            mock.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}