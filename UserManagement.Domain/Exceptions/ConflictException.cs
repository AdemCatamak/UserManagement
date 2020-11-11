using System;

namespace UserManagement.Domain.Exceptions
{
    public abstract class ConflictException : Exception
    {
        protected ConflictException(string message) : base(message)
        {
        }
    }
}