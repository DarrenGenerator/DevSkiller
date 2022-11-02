using System;

namespace Model
{
    public class MissingPostException : ApplicationException
    {
        public MissingPostException() { }

        public MissingPostException(Guid id) : base(string.Format("Post not found in repository: {0}", id.ToString())) { }
    }
}
