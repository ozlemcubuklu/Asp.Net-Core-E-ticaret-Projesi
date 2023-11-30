using System.Threading.Tasks;

namespace ShopApp.WebUi.EmailServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}
