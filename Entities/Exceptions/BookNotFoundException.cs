namespace Entities.Exceptions
{
    public sealed class BookNotFoundException : NotFoundException
    {
        //sealed class'ı kalıtıma kapar.Son hali budur yani.
        public BookNotFoundException(int id) : base($"The book with id:{id} could not found")
        {
        }
    }
}
