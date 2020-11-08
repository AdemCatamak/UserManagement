using System;

namespace UserManagement.Domain.Exceptions
{
    public abstract class ValidationException : Exception
    {
        protected ValidationException(string message) : base(message)
        {
        }
    }
}