using System;
using UserManagement.Domain.Services;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Application.Services
{
    public class UserIdGenerator : IUserIdGenerator
    {
        public UserId Generate()
        {
            var value = Guid.NewGuid().ToString();
            var userId = new UserId(value);
            return userId;
        }
    }
}