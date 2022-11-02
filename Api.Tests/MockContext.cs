using Microsoft.EntityFrameworkCore;
using Model;
using Repository;
using System;

namespace Api.Tests
{
    internal static class MockContext 
    {
        internal static Guid postTestId = Guid.NewGuid();
        internal static Guid commentTestId = Guid.NewGuid();
        internal static Guid commentTestId2 = Guid.NewGuid();

        internal static Post testPost = new() { Id = postTestId, Title = "Test Title", Content = "Test Post Content", CreationDate = DateTime.Now };
        internal static Post testPostUpdated = new() { Id = postTestId, Title = "Test Title Updated", Content = "Test Post Content", CreationDate = DateTime.Now };

        internal static Comment testComment = new(){ Id = commentTestId, PostId = postTestId, Author = "Test Author", Content = "Test Content", CreationDate = DateTime.Now };

        internal static BlogContext GetTestContext_WithPostAndComment()
        {
            string testDBName = string.Format("TestDB{0}", Guid.NewGuid().ToString());

            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: testDBName)
                .Options;

            var context = new BlogContext(options);

            //Add a test post
            context.Posts.Add(testPost);

            //Add a couple of comments to it
            context.Comments.Add(testComment);
            context.SaveChanges();

            return context;
        }

        internal static BlogContext GetTestContext_Empty()
        {
            string testDBName = string.Format("TestDB{0}", Guid.NewGuid().ToString());

            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: testDBName)
                .Options;

            return new BlogContext(options);
        }

    }
}
