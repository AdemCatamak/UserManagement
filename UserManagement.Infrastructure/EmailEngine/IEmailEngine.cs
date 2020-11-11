using System.Threading.Tasks;

namespace UserManagement.Infrastructure.EmailEngine
{
    public interface IEmailEngine
    {
        Task Send(EmailPost emailPost);
    }
}