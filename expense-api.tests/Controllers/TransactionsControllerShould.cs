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
    public class TransactionsControllerShould
    {
        private Transaction[] _transactions;
        public TransactionsControllerShould()
        {
            Transaction t1 = new Transaction() { Id = 900, Description = "Air Ticket (m)", Amount = 1270.00m, UserId = "user1" };
            Transaction t2 = new Transaction() { Id = 901, Description = "Business Meeting Exp (m)", Amount = 560.00m, UserId = "user1" };
            _transactions = new Transaction[] { t1, t2 };
        }

        [Fact]
        public async void TransactionsControllerGetReturnsAllTransactions()
        {
            // Arrange
            var mockRepository = new Mock<ITransactionRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(Task.FromResult<IEnumerable<Transaction>>(_transactions));
            var sut = new TransactionsController(mockRepository.Object);

            // Act
            IEnumerable<Transaction> result = await sut.Get();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(_transactions[0], result.First());
            Assert.Equal(_transactions[1], result.ElementAt(1));
        }

        [Fact]
        public async void TransactionsControllerGetByIdReturnsOneTransaction()
        {
            // Arrange
            var mockRepository = new Mock<ITransactionRepository>();
            mockRepository.Setup(repo => repo.GetById(1)).Returns(Task.FromResult<Transaction>(_transactions[0]));
            var sut = new TransactionsController(mockRepository.Object);

            // Act
            ActionResult<Transaction> result = await sut.Get(1);

            // Assert
            Assert.Equal(_transactions[0], result.Value);
        }

        [Fact]
        public async void TransactionsControllerGetByIdReturnsBadRequestWhenIdIsLessThanOne()
        {
            // Arrange
            var mockRepository = new Mock<ITransactionRepository>();
            var sut = new TransactionsController(mockRepository.Object);

            // Act
            ActionResult<Transaction> result = await sut.Get(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid request for the id 0", badRequestResult.Value);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void TransactionsControllerGetByIdReturnsNotFoundWhenIdNotFoundInDb()
        {
            // Arrange
            int tranId = 999;
            var mockRepository = new Mock<ITransactionRepository>();
            mockRepository.Setup(repo => repo.GetById(tranId)).Returns(Task.FromResult<Transaction>(null));

            var sut = new TransactionsController(mockRepository.Object);

            // Act
            ActionResult<Transaction> result = await sut.Get(tranId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Transaction not found for the provided id {tranId}", notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
