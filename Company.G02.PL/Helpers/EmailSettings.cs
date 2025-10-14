using System.Net;
using System.Net.Mail;

namespace Company.G02.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            // Mail Server  : Gmail
            // SMTP : Simple Mail Transfer Protocol
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("ayaragab115@gmail.com", "icteefhxlrpvapgx"); // sender   
                client.Send("ayaragab115@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
