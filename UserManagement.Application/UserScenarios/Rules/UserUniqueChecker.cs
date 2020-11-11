using System.Threading;
using System.Threading.Tasks;
using UserManagement.Domain.Aggregates.UserAggregate;
using UserManagement.Domain.Aggregates.UserAggregate.Rules;
using UserManagement.Domain.Exceptions;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Application.UserScenarios.Rules
{
    public class UserUniqueChecker : IUserUniqueChecker
    {
        private readonly IUserDbContext _userDbContext;

        public UserUniqueChecker(IUserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<bool> CheckAsync(Email email, CancellationToken cancellationToken)
        {
            var unique = false;

            var userRepository = _userDbContext.UserRepository;
            try
            {
                await userRepository.GetUserAsync(email, cancellationToken);
            }
            catch (NotFoundException<User>)
            {
                unique = true;
            }

            return unique;
        }
    }
}