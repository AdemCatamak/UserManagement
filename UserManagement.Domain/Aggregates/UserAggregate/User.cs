using System;
using System.Threading;
using UserManagement.Domain.Aggregates.UserAggregate.Events;
using UserManagement.Domain.Aggregates.UserAggregate.Rules;
using UserManagement.Domain.Exceptions.Imp;
using UserManagement.Domain.Services;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Aggregates.UserAggregate
{
    public class User : Aggregate
    {
        public UserId UserId { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public PasswordHash PasswordHash { get; private set; } = null!;
        public DateTime CreatedOn { get; private set; }

        private User()
        {
            // Only for EF
        }

        private User(UserId userId, Email email, PasswordHash passwordHash, DateTime createdOn)
        {
            UserId = userId;
            Email = email;
            PasswordHash = passwordHash;
            CreatedOn = createdOn;
        }

        public static User Create(Email email, IUserIdGenerator userIdGenerator, IUserUniqueChecker userUniqueChecker, IPasswordGenerator passwordGenerator, IPasswordHasher passwordHasher, CancellationToken cancellationToken)
        {
            if (!email.IsValid) throw new EmailNotValidException(email);
            bool unique = userUniqueChecker.CheckAsync(email, cancellationToken).GetAwaiter().GetResult();
            if (!unique) throw new UserAlreadyExistException(email);

            UserId userId = userIdGenerator.Generate();
            Password password = passwordGenerator.Generate();
            PasswordHash passwordHash = passwordHasher.Hash(password);
            var user = new User(userId, email, passwordHash, DateTime.UtcNow);
            var userCreatedEvent = new UserCreatedEvent(user, password);

            user.AddDomainEvent(userCreatedEvent);
            return user;
        }
    }
}