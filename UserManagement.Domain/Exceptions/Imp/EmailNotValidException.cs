using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Exceptions.Imp
{
    public class EmailNotValidException : ValidationException
    {
        public EmailNotValidException(Email email) : base($"{email} is not valid email")
        {
        }
    }
}