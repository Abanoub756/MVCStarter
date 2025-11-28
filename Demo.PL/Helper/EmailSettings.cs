using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            // Implement email sending logic here
            // This is a placeholder for demonstration purposes

            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("abanoubsamy756@gmail.com", "tucl ynix ethl yria");

            client.Send("abanoubsamy756@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
