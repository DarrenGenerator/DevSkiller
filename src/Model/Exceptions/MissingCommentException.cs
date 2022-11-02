using System;

namespace Model
{
    public class MissingCommentException : ApplicationException
    {
        public MissingCommentException() { }

        public MissingCommentException(Guid id)
            : base(string.Format("Comment not found: {0}", id.ToString()))
        {

        }
    }
}
