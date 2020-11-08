using System;

namespace UserManagement.Domain.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message)
        {
        }
    }

    public class NotFoundException<T> : NotFoundException
    {
        public NotFoundException() : base($"{typeof(T).Name} could not found")
        {
        }
    }
}