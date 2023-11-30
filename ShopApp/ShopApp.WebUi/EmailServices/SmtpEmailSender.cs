using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ShopApp.WebUi.EmailServices
{
    public class SmtpEmailSender : IEmailSender
    {
        string _host;
        int _port;
        string _username;
        string _password;
        bool _enableSSL;

        public SmtpEmailSender(string host,int port,bool enableSSL,string username,string password)
        {

            this._enableSSL = enableSSL;
            this._host = host;
            this._port = port;
            this._username = username;
            this._password = password;
        }
        public Task SendEmailAsync(string email, string subject, string body)
        {
            var client = new SmtpClient(_host, _port) {
                Credentials=new NetworkCredential(_username,_password),
                EnableSsl = this._enableSSL
            };
            return client.SendMailAsync(new MailMessage(this._username, email, subject, body){
                IsBodyHtml = true
            });

        }
    }
}
