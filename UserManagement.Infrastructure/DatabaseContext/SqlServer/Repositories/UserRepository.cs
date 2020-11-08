using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Aggregates.UserAggregate;
using UserManagement.Domain.Exceptions;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Infrastructure.DatabaseContext.SqlServer.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly EfDbContext _efDbContext;

        public UserRepository(EfDbContext efDbContext)
        {
            _efDbContext = efDbContext;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _efDbContext.AddAsync(user, cancellationToken);
        }

        public async Task<User> GetUserAsync(Email email, CancellationToken cancellationToken)
        {
            var user = await _efDbContext.Set<User>()
                                         .FirstOrDefaultAsync(x => x.Email.Value == email.Value, cancellationToken);

            if (user == null)
                throw new NotFoundException<User>();

            return user;
        }
    }
}