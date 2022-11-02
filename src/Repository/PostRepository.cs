using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Repository
{
    public class PostRepository
    {
        private readonly BlogContext _blogContext;

        public PostRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public IEnumerable<Post> GetAll()
        {
            return _blogContext.Posts;
        }

        public Post Get(Guid id)
        {
            Post result = _blogContext.Posts.Where(p => p.Id == id).FirstOrDefault();

            if (result == null)
            {
                throw new MissingPostException(id);
            }

            return result;
        }

        public bool Exists(Guid id)
        {
            Post result = _blogContext.Posts.Where(p => p.Id == id).FirstOrDefault();

            return result != null;
        }

        public Post Create(Post post)
        {
            if (Exists(post.Id))
            {
                throw new PostAlreadyExists(post.Id);
            }

            _blogContext.Posts.Add(post);
            _blogContext.SaveChanges();

            return _blogContext.Posts.Where(c => c.Id == post.Id).FirstOrDefault();
        }

        public Post Update(Guid id, Post source)
        {
            Post targetPost = _blogContext.Posts.Where(p => p.Id == id).FirstOrDefault();

            if (targetPost == null)
            {
                throw new MissingPostException(id);
            }

            targetPost.Id = source.Id;
            targetPost.Title = source.Title;
            targetPost.Content = source.Content;
            targetPost.CreationDate = source.CreationDate;

            _blogContext.SaveChanges();

            Post updatedResult = _blogContext.Posts.Where(p => p.Id == id).FirstOrDefault();

            return updatedResult;
        }

        public bool Delete(Guid id)
        {
            Post targetPost = _blogContext.Posts.Where(c => c.Id == id).FirstOrDefault();

            if (targetPost == null)
            {
                return false;
            }

            _blogContext.Posts.Remove(targetPost);
            _blogContext.SaveChanges();

            return true;
        }
    }
}