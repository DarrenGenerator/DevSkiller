using System;

namespace Model
{
    public class PostAlreadyExists : ApplicationException
    {
        public PostAlreadyExists() { }

        public PostAlreadyExists(Guid id)
            : base(string.Format("Cannot create - Post already exists: {0}", id.ToString()))
        {

        }
    }
}
