using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.UserScenarios.Commands;
using UserManagement.Controllers.Requests;
using UserManagement.Domain.Services.DomainMessageBroker;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Controllers
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IDomainMessageBroker _domainMessageBroker;

        public UserController(IDomainMessageBroker domainMessageBroker)
        {
            _domainMessageBroker = domainMessageBroker;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserId), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> PostUser([FromBody] PostUserHttpRequest? postUserHttpRequest)
        {
            var createUserCommand = new CreateUserCommand(new Email(postUserHttpRequest?.Email ?? string.Empty));
            var userId = await _domainMessageBroker.SendAsync(createUserCommand, CancellationToken.None);
            return StatusCode((int) HttpStatusCode.Created, userId);
        }
    }
}