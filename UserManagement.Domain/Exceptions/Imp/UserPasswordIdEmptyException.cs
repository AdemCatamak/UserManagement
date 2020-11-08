namespace UserManagement.Domain.Exceptions.Imp
{
    public class UserPasswordIdEmptyException : ValidationException
    {
        public UserPasswordIdEmptyException() : base("User password identifier should not be empty")
        {
        }
    }
}