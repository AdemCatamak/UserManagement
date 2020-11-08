using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.UserScenarios.Commands;
using UserManagement.Domain.Aggregates.UserAggregate;
using UserManagement.Domain.Aggregates.UserAggregate.Rules;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Services;
using UserManagement.Domain.Services.DomainMessageBroker;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Application.UserScenarios.CommandHandlers
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserId>
    {
        private readonly IUserDbContext _dbContext;
        private readonly IUserIdGenerator _userIdGenerator;
        private readonly IUserUniqueChecker _userUniqueChecker;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserDbContext dbContext, IUserIdGenerator userIdGenerator, IUserUniqueChecker userUniqueChecker, IPasswordGenerator passwordGenerator, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _userIdGenerator = userIdGenerator;
            _userUniqueChecker = userUniqueChecker;
            _passwordGenerator = passwordGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserId> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(request.Email, _userIdGenerator, _userUniqueChecker, _passwordGenerator, _passwordHasher, cancellationToken);

            var userRepository = _dbContext.UserRepository;
            await userRepository.AddAsync(user, cancellationToken);
            await _dbContext.SaveAsync(cancellationToken);

            return user.UserId;
        }
    }
}