using Company.G02.PL.Setting;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.G02.PL.Helpers
{
    public class MailService(IOptions<MailSettings> _options) : IMailService
    {
        //private readonly MailSettings _options;
        //public MailService(IOptions<MailSettings> options)
        //{
        //    _options = options.Value;
        //}
        public void SendEmail(Email email)
        {
            //Bulid Message
            var mail = new MimeMessage();
            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress( _options.Value.DisplayName,_options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            builder.TextBody= email.Body;
            mail.Body = builder.ToMessageBody();

            //Establish Connection with SMTP Server

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Email, _options.Value.Password);

            //Send Email

            smtp.Send(mail);

        }
    }
}
