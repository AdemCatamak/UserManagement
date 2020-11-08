using System;
using System.Threading.Tasks;

namespace UserManagement.Infrastructure.EmailEngine
{
    public class ConsoleEmailEngine : IEmailEngine
    {
        public Task Send(EmailPost emailPost)
        {
            Console.WriteLine(emailPost.ToString());

            return Task.CompletedTask;
        }
    }
}