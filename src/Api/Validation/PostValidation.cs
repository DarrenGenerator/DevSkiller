using Model;
using Repository;

namespace Api.Validation
{
    public static class PostValidation
    {
        public static void Validate (this Post validatingPost) 
        {
            if (validatingPost.Title.Length > 30)
            {
                throw new BadDataException(string.Format("Title is too long"));
            }

            if (validatingPost.Content.Length > 1200)
            {
                throw new BadDataException(string.Format("Content is too long"));
            }
        }
    }
}
