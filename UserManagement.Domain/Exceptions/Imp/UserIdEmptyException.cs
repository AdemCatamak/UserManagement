namespace UserManagement.Domain.Exceptions.Imp
{
    public class UserIdEmptyException : ValidationException
    {
        public UserIdEmptyException() : base("User identifier should not be empty")
        {
        }
    }
}