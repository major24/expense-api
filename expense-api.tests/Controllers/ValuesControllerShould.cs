using System.Linq;
using Xunit;
using expense_api.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace expense_api.tests
{
    public class ValuesControllerShould
    {
        [Fact]
        public void ValuesControllerGetReturnValues()
        {
            // Arrange
            var sut = new ValuesController();

            // Act
            ActionResult<IEnumerable<string>> result = sut.Get();

            // Assert
            IEnumerable<string> resultValue = result.Value;

            Assert.Equal(2, resultValue.Count());
            Assert.Equal("value1", resultValue.First());
            Assert.Equal("value2", resultValue.ElementAt(1));
        }

        
        [Fact]
        public void ValuesControllerGetReturnValueById()
        {
            // Arrange
            var sut = new ValuesController();

            // Act
            ActionResult result = sut.Get(1);

            // Assert
            var contentRequestResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Value submitted: 1", contentRequestResult.Content);
        }

        [Fact]
        public void ValuesControllerGetReturnsErrorWhenIdLessThanOne()
        {
            // Arrange
            var sut = new ValuesController();

            // Act
            ActionResult result = sut.Get(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request for the id 0", badRequestResult.Value);
        }

        [Fact]
        public void ValuesControllerPostStartBatchJobReturnsSuccess()
        {
            // Arrange
            var sut = new ValuesController();

            // Act
            IActionResult result = sut.StartBatchJob();

            // Assert
            var okRequestResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Batch job started.", okRequestResult.Value);
        }
    }
}
