using System;

namespace Model
{
    public class BadDataException : ApplicationException
    {
        public BadDataException() { }

        public BadDataException(string message) : base(string.Format("Submitted data is invalid: {0}", message)) { }
    }
}
