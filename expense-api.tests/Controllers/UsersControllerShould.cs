using System.Linq;
using Xunit;
using expense_api.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using expense_api.Models;
using Moq;
using expense_api.Repositories;

namespace expense_api.tests.Controllers
{
    public class UsersControllerShould
    {
        private User[] _users;
        public UsersControllerShould()
        {
            User u1 = new User() { UserId = "user1", FirstName = "Major", LastName = "Nalliah", CostCentre = "ITECH", ManagerId = "user4", Active = "Y" };
            User u2 = new User() { UserId = "user2", FirstName = "Kumaran", LastName = "Senthil", CostCentre = "ITECH", ManagerId = "user4", Active = "Y" };
            User u3 = new User() { UserId = "user3", FirstName = "James", LastName = "Camaron", CostCentre = "MRKTN", ManagerId = "user6", Active = "N" };
            User u4 = new User() { UserId = "user3", FirstName = "Steve", LastName = "Richard", CostCentre = "ITECH", ManagerId = "user5", Active = "Y" };
            _users = new User[] { u1, u2, u3, u4 };
        }

        // async Task<IEnumerable<User>> GetAllUsers()
        [Fact]
        public async void UsersControllerGetReturnsAllUsers()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(Task.FromResult<IEnumerable<User>>(_users));
            var sut = new UsersController(mockRepository.Object);

            // Act
            IEnumerable<User> result = await sut.GetAllUsers();

            // Assert
            Assert.Equal(4, result.Count());
            Assert.Equal(_users[0], result.First());
            Assert.Equal(_users[1], result.ElementAt(1));
        }

        [Fact]
        public async void UsersControllerGetByIdReturnsOneUser()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetById("user1")).Returns(Task.FromResult<User>(_users[0]));
            var sut = new UsersController(mockRepository.Object);

            // Act
            ActionResult<User> result = await sut.GetByUserId("user1");

            // Assert
            Assert.Equal(_users[0], result.Value);
        }

        [Fact]
        public async void UsersControllerGetByIdReturnsNotFoundWhenIdNotFoundInDb()
        {
            // Arrange
            string userId = "user999";
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetById(userId)).Returns(Task.FromResult<User>(null));
            var sut = new UsersController(mockRepository.Object);

            // Act
            ActionResult<User> result = await sut.GetByUserId(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"User not found for the provided id {userId}", notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void UsersControllerGetByIdReturnsBadRequestWhenIdIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var sut = new UsersController(mockRepository.Object);

            // Act
            ActionResult<User> result = await sut.GetByUserId(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Must provide a user id", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void UsersControllerGetByIdReturnsBadRequestWhenIdIsEmpty()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var sut = new UsersController(mockRepository.Object);

            // Act
            ActionResult<User> result = await sut.GetByUserId("");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Must provide a user id", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

    }
}
