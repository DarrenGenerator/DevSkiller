using System;

namespace Model
{
    public class CommentAlreadyExists : ApplicationException
    {
        public CommentAlreadyExists() { }

        public CommentAlreadyExists(Guid id)
            : base(string.Format("Cannot create - Comment already exists: {0}", id.ToString()))
        {

        }
    }
}
