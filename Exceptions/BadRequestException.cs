
namespace ParserToyRu.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
            : base("Bad adress request!")
        { 
        }

        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
