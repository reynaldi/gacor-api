using System.Linq.Expressions;
using GacorAPI.Data.Entities;
using GacorAPI.Data.Repositories;
using GacorAPI.Data.Uow;
using GacorAPI.Domain.UserDom;
using GacorAPI.Domain.UserDom.Dto;
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
            mockRepo.Setup(_ => _.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedEnumerable<User>>>(), It.IsAny<string>())).ReturnsAsync(new List<User>());

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
            var err = await userService.CreateUser(userDto);

            // assert
            mockRepo.Verify(x => x.InsertAsync(It.Is<User>(arg => arg.Email == userDto.Email && arg.IsActive)), Times.Once);
            mock.Verify(x => x.SaveAsync(), Times.Once);
            mock.Verify(_ => _.CompleteTransaction(), Times.Once);
            Assert.Null(err);
        }

        [Fact]
        public async Task InsertUser_DuplicateError()
        {
            // arrange
            var mockRepo = new Mock<IGeneralRepository<User>>();
            mockRepo.Setup(x => x.InsertAsync(It.IsAny<User>()));
            mockRepo.Setup(_ => _.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedEnumerable<User>>>(), It.IsAny<string>())).ReturnsAsync(new List<User>(){ new User{ Email = "reynaldi.surya@gmail.com" } });

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
            var err = await userService.CreateUser(userDto);

            // assert
            Assert.NotNull(err);
            Assert.Equal("User already registered", err.Message);
        }

        [Fact]
        public async Task InsertUser_ThrowException()
        {
            // arrange
            var mockRepo = new Mock<IGeneralRepository<User>>();
            mockRepo.Setup(x => x.InsertAsync(It.IsAny<User>())).Throws(new Exception("new exception"));
            mockRepo.Setup(_ => _.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedEnumerable<User>>>(), It.IsAny<string>())).ReturnsAsync(new List<User>());

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
            Func<Task> act = () => userService.CreateUser(userDto);
            var ex = await Record.ExceptionAsync(act);

            // assert
            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
            mock.Verify(_ => _.DisposeTransaction(), Times.Once);
        }
    }
}