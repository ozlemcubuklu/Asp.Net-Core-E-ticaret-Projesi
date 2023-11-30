using ShopApp.WebUi.EmailServices;

namespace shopapp.webui
{
    internal class EmailSender : SmtpEmailSender
    {
        public EmailSender(string host, int port, bool enableSSL, string username, string password) : base(host, port, enableSSL, username, password)
        {
        }
    }
}