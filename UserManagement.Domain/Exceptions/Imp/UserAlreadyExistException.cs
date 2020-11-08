using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Exceptions.Imp
{
    public class UserAlreadyExistException : ConflictException
    {
        public UserAlreadyExistException(Email email) : base($"User is already registered in the system with {email} mail address")
        {
        }
    }
}