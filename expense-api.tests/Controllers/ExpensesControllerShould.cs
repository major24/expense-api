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
    public class ExpensesControllerShould
    {
        private ExpenseItem[] _expenses;
        public ExpensesControllerShould()
        {
            ExpenseItem expenseItem = new ExpenseItem() { Description = "My new expense", Amount = 99.50M, Tax = 25.99M, Category = "TRVL", TransDate = System.DateTime.Now };
            _expenses = new ExpenseItem[] { expenseItem };
        }

        [Fact]
        public void ExpensesControllerPostExpensesReturnsExpenseId()
        {
            // Arrange
            Expense expense = new Expense() { UserId = "user1", CostCentre = "ITECH", ApproverId = "user5", Status = "Submitted", SubmittedDate = System.DateTime.Now, ExpenseItems = _expenses };

            var mockRepository = new Mock<IExpenseRepository>();
            mockRepository.Setup(repo => repo.Save(expense)).Returns(Task.FromResult<int>(24));
            var sut = new ExpensesController(mockRepository.Object);

            // Act
            // var result = sut.Post(expense);
            // Assert
            //var createdResult = Assert.IsType<CreatedResult>(result.Result);
            // Act
            var result = sut.Post(expense);

            // Assert
            Assert.Equal(24, result.Result.Value.Id);
            Assert.Equal("Expense has been created", result.Result.Value.Message);
        }

        [Fact]
        public void ExpensesControllerPostReturnsBadRequestWhenUserIdIsMissing()
        {
            // Arrange
            Expense expense = new Expense() { UserId = "", CostCentre = "ITECH", ApproverId = "user5", Status = "Submitted", SubmittedDate = System.DateTime.Now, ExpenseItems = _expenses };

            var mockRepository = new Mock<IExpenseRepository>();
            mockRepository.Setup(repo => repo.Save(expense)).Returns(Task.FromResult<int>(24));
            var sut = new ExpensesController(mockRepository.Object);

            // Act
            var result = sut.Post(expense);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal("Missing required field(s) for expense request", badRequestObjectResult.Value);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public void ExpensesControllerPostReturnsBadRequestWhenApproverIdIsMissing()
        {
            // Arrange
            Expense expense = new Expense() { UserId = "user1", CostCentre = "ITECH", ApproverId = "", Status = "Submitted", SubmittedDate = System.DateTime.Now, ExpenseItems = _expenses };

            var mockRepository = new Mock<IExpenseRepository>();
            mockRepository.Setup(repo => repo.Save(expense)).Returns(Task.FromResult<int>(24));
            var sut = new ExpensesController(mockRepository.Object);

            // Act
            var result = sut.Post(expense);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal("Missing required field(s) for expense request", badRequestObjectResult.Value);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public void ExpensesControllerPostReturnsBadRequestWhenCostCenterIdIsMissing()
        {
            // Arrange
            Expense expense = new Expense() { UserId = "user1", CostCentre = null, ApproverId = "user5", Status = "Submitted", SubmittedDate = System.DateTime.Now, ExpenseItems = _expenses };

            var mockRepository = new Mock<IExpenseRepository>();
            mockRepository.Setup(repo => repo.Save(expense)).Returns(Task.FromResult<int>(24));
            var sut = new ExpensesController(mockRepository.Object);

            // Act
            var result = sut.Post(expense);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal("Missing required field(s) for expense request", badRequestObjectResult.Value);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public void ExpensesControllerPostReturnsBadRequestWhenExenseItemIsMissing()
        {
            // Arrange
            Expense expense = new Expense() { UserId = "user1", CostCentre = "ITECH", ApproverId = "user5", Status = "Submitted", SubmittedDate = System.DateTime.Now, ExpenseItems = null };

            var mockRepository = new Mock<IExpenseRepository>();
            mockRepository.Setup(repo => repo.Save(expense)).Returns(Task.FromResult<int>(24));
            var sut = new ExpensesController(mockRepository.Object);

            // Act
            var result = sut.Post(expense);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal("Should submit atleast one expense item", badRequestObjectResult.Value);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public void ExpensesControllerPostReturnsBadRequestWhenExenseItemIsEmpty()
        {
            // Arrange
            Expense expense = new Expense() { UserId = "user1", CostCentre = "ITECH", ApproverId = "user5", Status = "Submitted", SubmittedDate = System.DateTime.Now, ExpenseItems = new ExpenseItem[0] };

            var mockRepository = new Mock<IExpenseRepository>();
            mockRepository.Setup(repo => repo.Save(expense)).Returns(Task.FromResult<int>(24));
            var sut = new ExpensesController(mockRepository.Object);

            // Act
            var result = sut.Post(expense);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal("Should submit atleast one expense item", badRequestObjectResult.Value);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

    }
}
