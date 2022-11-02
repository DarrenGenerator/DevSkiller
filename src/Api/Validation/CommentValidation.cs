using Model;
using Repository;
using System;

namespace Api.Validation
{
    public static class CommentValidation
    {
        public static void Validate(this Comment validatingComment, BlogContext blogContext)
        {
            if (validatingComment.Author.Length > 30)
            {
                throw new BadDataException(string.Format("Author is too long"));
            }

            if (validatingComment.Content.Length > 120)
            {
                throw new BadDataException(string.Format("Content is too long"));
            }

            //Hmm, this would hit the DB for every Post - I wouldn't do this in live ... 
            //I added an Exists method to the Repository anyway
            PostRepository repository = new(blogContext);

            Guid postId = validatingComment.PostId;
            if (!repository.Exists(postId))
            {
                throw new MissingPostException(postId);
            }
        }
    }
}
