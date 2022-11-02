using System;
using System.Collections.Generic;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;
using Xunit;

using static Api.Tests.MockContext;

namespace Api.Tests
{
    //Notes - I could write a few more tests, at least, testing Validation of requests etc.
    //I don't think it's necessary to go that far ... let me know if you want me to :)

    public class PostControllerTests
    {
        [Fact]
        public void GetAll_Returns_Existing_Posts()
        {
            var context = GetTestContext_Empty();

            // Arrange
            var expected = new List<Post>();

            // Act
            var actual = new PostController(null, context).GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            Assert.Equal(expected, okObjectResult.Value);
        }

        [Fact]
        public void Get_Single_Return_Existing_Post()
        {
            // Arrange
            var context = GetTestContext_WithPostAndComment();

            var expected = testPost;

            // Act
            var actual = new PostController(null, context).Get(testPost.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            Assert.Equal(expected, okObjectResult.Value);
        }

        [Fact]
        public void Post_Creates_One_PostItem()
        {
            // Arrange
            var context = GetTestContext_Empty();

            Guid postId = Guid.NewGuid();
            Post test = new()
            {
                Id = postId,
                Title = "Test Title",
                Content = "test Content",
                CreationDate = DateTime.Now
            };

            Post expected = test;

            // Act
            var actual = new PostController(null, context).Post(test);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);

            Assert.Equal(expected, okObjectResult.Value);
        }

        [Fact]
        public void Put_Updates_One_PostItem()
        {
            // Arrange
            var context = GetTestContext_WithPostAndComment();

            Post test = testPostUpdated;
            Post expected = testPostUpdated;

            // Act
            var actual = new PostController(null, context).Put(test.Id, test);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);

            Assert.Equal(expected, okObjectResult.Value);
        }

        [Fact]
        public void Delete_Removes_One_PostItem()
        {
            // Arrange
            var context = GetTestContext_WithPostAndComment();

            // Act
            IActionResult actionResult = new PostController(null, context).Delete(testPost.Id);

            //Assert
            Assert.IsAssignableFrom<ActionResult>(actionResult); //Hmm, return type is IActionResult, not ActionResult on this API method
            ActionResult result = (ActionResult)actionResult;

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<bool>(((OkObjectResult)actionResult).Value);
            Assert.True((bool)okObjectResult.Value, "Expected true as result");
        }

        [Fact]
        public void GetComments_Returns_Expected_Comments()
        {
            // Arrange
            var context = GetTestContext_WithPostAndComment();
            List<Comment> expected = new() { testComment }; //Expecting List of comment(s)

            // Act
            var actionResult = new PostController(null, context).GetComments(testPost.Id);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult.Result);
            
            var result = (OkObjectResult)actionResult.Result;
            Assert.IsType<List<Model.Comment>>(result.Value);
            Assert.Equal(expected, result.Value);
        }
    }
}