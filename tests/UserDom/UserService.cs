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

        [Fact]
        public async Task GetUserProfile_Valid()
        {
            // arrange
            var mockRepo = new Mock<IGeneralRepository<User>>();
            var mockUow = new Mock<IUnitOfWork>();
            mockRepo.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(new User 
            { 
                Email = "reynaldi.surya@gmail.com",
                Id = 1,
                FirstName = "Reynaldi",
                LastName = "Surya",                
            });
            mockUow.Setup(_ => _.UserRepository()).Returns(mockRepo.Object);

            // act
            var service = new UserService(mockUow.Object);
            var result = await service.GetUserProfile("reynaldi.surya@gmail.com");

            // assert
            Assert.NotNull(result.Item1);
            Assert.Equal("reynaldi.surya@gmail.com", result.Item1.Email);
        }

        [Fact]
        public async Task GetUserProfile_UserNotFound()
        {
            // arrange
            var mockRepo = new Mock<IGeneralRepository<User>>();
            var mockUow = new Mock<IUnitOfWork>();
            mockRepo.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(value: null as User);
            mockUow.Setup(_ => _.UserRepository()).Returns(mockRepo.Object);

            // act
            var service = new UserService(mockUow.Object);
            var result = await service.GetUserProfile("reynaldi.surya@gmail.com");

            // assert
            Assert.NotNull(result.Item2);
            Assert.Equal("User not found", result.Item2.Message);
        }

        [Fact]
        public async Task EditUser_Valid()
        {
            // arrange
            var mockRepo = new Mock<IGeneralRepository<User>>();
            var mockUow = new Mock<IUnitOfWork>();

            mockRepo.Setup(_ => _.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(new User
            {
                Id = 1,
                FirstName = "Reynaldi",
                LastName = "Surya",
                Email = "reynaldi.surya@gmail.com",
                IsActive = true
            });
            mockUow.Setup(_ => _.UserRepository()).Returns(mockRepo.Object);

            // act
            var service = new UserService(mockUow.Object);
            var result = await service.EditUser(new UserDto
            { 
                FirstName = "reynaldi testt",
                LastName = "Surya tess",
                Id = 1
            });

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task EditUser_NotFound()
        {
            // arrange
            var mockRepo = new Mock<IGeneralRepository<User>>();
            var mockUow = new Mock<IUnitOfWork>();

            mockRepo.Setup(_ => _.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(value: null as User);
            mockUow.Setup(_ => _.UserRepository()).Returns(mockRepo.Object);

            // act
            var service = new UserService(mockUow.Object);
            var result = await service.EditUser(new UserDto
            { 
                FirstName = "reynaldi testt",
                LastName = "Surya tess",
                Id = 1
            });

            // assert
            Assert.NotNull(result);
            Assert.Equal("User not found", result.Message);
        }
    }
}