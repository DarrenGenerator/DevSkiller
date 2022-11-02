using System;
using System.Collections.Generic;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Model;
using Xunit;

using static Api.Tests.MockContext;

namespace Api.Tests
{
    public class CommentControllerTests
    {
        //Notes - I could write a few more tests, at least, testing Validation of requests etc.
        //I don't think it's necessary to go that far ... let me know if you want me to :)

        [Fact]
        public void GetAll_Returns_Existing_Comments()
        {
            var context = GetTestContext_Empty();

            // Arrange
            var expected = new List<Comment>();

            // Act
            var actual = new CommentController(null, context).GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            Assert.Equal(expected, okObjectResult.Value);
        }

        [Fact]
        public void Get_Single_Return_Existing_Comment()
        {
            // Arrange
            var context = GetTestContext_WithPostAndComment();

            var expected = testComment;

            // Act
            var actual = new CommentController(null, context).Get(testComment.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            Assert.Equal(expected, okObjectResult.Value);
        }

        [Fact]
        public void Post_Creates_One_CommentItem()
        {
            // Arrange
            var context = GetTestContext_WithPostAndComment();

            Comment test = new()
            {
                Id = Guid.NewGuid(),
                PostId = testPost.Id, //Existing post within the text context that this comment will be added to
                Author = "Test Author",
                Content = "Test Content",
                CreationDate = DateTime.Now
            };

            Comment expected = test;

            // Act
            var actual = new CommentController(null, context).Post(test);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);

            Assert.Equal(expected, okObjectResult.Value);
        }

        [Fact]
        public void Post_Updates_One_CommentItem()
        {
            // Arrange
            var context = GetTestContext_WithPostAndComment();

            Comment testUpdate = testComment;
            testUpdate.Content = "Updated Test Content";

            Comment expected = testUpdate;

            // Act
            var actual = new CommentController(null, context).Put(testUpdate.Id, testUpdate);

            // Assert
            Assert.IsAssignableFrom<ActionResult>(actual); //Hmm, return type is IActionResult, not ActionResult
            ActionResult result = (ActionResult)actual;

            var okObjectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(expected, okObjectResult.Value);
        }

        [Fact]
        public void Delete_Removes_One_CommentItem()
        {
            // Arrange
            var context = GetTestContext_WithPostAndComment();

            // Act
            IActionResult actionResult = new CommentController(null, context).Delete(testComment.Id);

            //Assert
            Assert.IsAssignableFrom<ActionResult>(actionResult); //Hmm, return type is IActionResult, not ActionResult on this API method
            ActionResult result = (ActionResult)actionResult;
            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<bool>(((OkObjectResult)actionResult).Value);
            Assert.True((bool)okObjectResult.Value, "Expected true as result");
        }
    }
}