using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Services.DomainMessageBroker;

namespace UserManagement.Controllers
{
    [Microsoft.AspNetCore.Components.Route("")]
    public class HomeController : ControllerBase
    {
        private readonly IUserDbContext _userDbContext;


        public HomeController(IUserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [HttpGet("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Home()
        {
            return Redirect($"{Request.Scheme}://{Request.Host.ToUriComponent()}/swagger");
        }

        [HttpGet("health-check")]
        public IActionResult HealthCheck()
        {
            var response = new {Environment.MachineName};
            return StatusCode((int) HttpStatusCode.OK, response);
        }

        [HttpGet("failed-job-count")]
        public async Task<IActionResult> GetFailedJobCount()
        {
            int failedJobCount = await _userDbContext.OutboxMessageRepository.GetFailedJobCountAsync(CancellationToken.None);
            return StatusCode((int) HttpStatusCode.OK, failedJobCount);
        }
    }
}