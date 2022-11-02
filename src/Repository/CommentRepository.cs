using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Repository
{
    public class CommentRepository
    {
        BlogContext _blogContext;

        public CommentRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public IEnumerable<Comment> GetAll()
        {
            return _blogContext.Comments;
        }

        public Comment Get(Guid id)
        {
            Comment result = _blogContext.Comments.Where(c => c.Id == id).FirstOrDefault();

            if (result == null)
            {
                throw new MissingCommentException(id);
            }

            return result;
        }

        public Comment Create(Comment comment)
        {
            if (Exists(comment.Id))
            {
                throw new CommentAlreadyExists(comment.Id);
            }

            _blogContext.Comments.Add(comment);
            _blogContext.SaveChanges();

            return _blogContext.Comments.Where(c => c.Id == comment.Id).FirstOrDefault();
        }

        public Comment Update(Guid id, Comment source)
        {
            Comment target = _blogContext.Comments.Where(c => c.Id == id).FirstOrDefault();

            if (target == null)
            {
                string message = string.Format("Target Comment not found: {0}", id.ToString());
                throw new MissingCommentException(id);
            }

            target.Id = source.Id;
            target.PostId = source.PostId;
            target.Content = source.Content;
            target.Author = source.Author;
            target.CreationDate = source.CreationDate;

            _blogContext.SaveChanges();

            Comment updatedResult = _blogContext.Comments.Where(c => c.Id == id).FirstOrDefault();

            return updatedResult;
        }

        public bool Delete(Guid targetId)
        {
            Comment target = _blogContext.Comments.Where(c => c.Id == targetId).FirstOrDefault();

            if(target == null)
            {
                return false;
            }

            _blogContext.Comments.Remove(target);
            _blogContext.SaveChanges();

            return true;
        }

        public IEnumerable<Comment> GetByPostId(Guid postId)
        {
            return _blogContext.Comments.Where(c => c.PostId == postId).ToList();
        }

        public bool Exists(Guid id)
        {
            Comment result = _blogContext.Comments.Where(c => c.Id == id).FirstOrDefault();

            return result != null;
        }
    }
}