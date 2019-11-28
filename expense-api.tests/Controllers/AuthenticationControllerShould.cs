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
    public class AuthenticationControllerShould
    {
        private UserCredentials[] _userCredentials;

        public AuthenticationControllerShould()
        {
            UserCredentials u1 = new UserCredentials() { UserId = "user1", Password = "111" };
            UserCredentials u2 = new UserCredentials() { UserId = "user2", Password = "222" };
            _userCredentials = new UserCredentials[] { u1, u2 };
        }

        [Fact]
        public void AuthenticationControllerPostReturnsTrueForValidUser()
        {
            // Arrange
            UserCredentials uc = _userCredentials[0];
            var mockRepository = new Mock<IAuthenticationRepository>();
            mockRepository.Setup(repo => repo.IsValidUser("user1", "111")).Returns(Task.FromResult<bool>(true));
            var sut = new AuthenticationController(mockRepository.Object);

            // Act
            var result = sut.Post(uc);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("Authentication Success", okObjectResult.Value);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public void AuthenticationControllerPostReturnsFalseForInValidUserName()
        {
            // Arrange
            UserCredentials uc = new UserCredentials() { UserId = "user999", Password = "111" };
            var mockRepository = new Mock<IAuthenticationRepository>();
            mockRepository.Setup(repo => repo.IsValidUser("user999", "111")).Returns(Task.FromResult<bool>(false));
            var sut = new AuthenticationController(mockRepository.Object);

            // Act
            var result = sut.Post(uc);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Authentication Failed", notFoundObjectResult.Value);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }
       
        [Fact]
        public void AuthenticationControllerPostReturnsBadRequestWhenUserNameMissing()
        {
            // Arrange
            UserCredentials uc = new UserCredentials() { UserId = "", Password = "111" };
            var mockRepository = new Mock<IAuthenticationRepository>();
            var sut = new AuthenticationController(mockRepository.Object);

            // Act
            var result = sut.Post(uc);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Missing required field(s) for user authentication", badRequestObjectResult.Value);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public void AuthenticationControllerPostReturnsBadRequestWhenUserPasswordMissing()
        {
            // Arrange
            UserCredentials uc = new UserCredentials() { UserId = "user1", Password = null };
            var mockRepository = new Mock<IAuthenticationRepository>();
            var sut = new AuthenticationController(mockRepository.Object);

            // Act
            var result = sut.Post(uc);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Missing required field(s) for user authentication", badRequestObjectResult.Value);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

    }
}
