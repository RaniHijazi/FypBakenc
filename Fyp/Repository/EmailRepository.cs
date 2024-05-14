using Fyp.Interfaces;
using Fyp.Dto;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Fyp.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _config;
        private readonly Random _random;
        private readonly DataContext _context;
        public EmailRepository(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
            _random = new Random();
        }

        private string GenerateVerificationCode()
        {
            return _random.Next(1000, 9999).ToString();
        }

        public void SendVerificationCode(string userEmail)
        {

            string verificationCode = GenerateVerificationCode();
            var user = _context.users.SingleOrDefault(u => u.Email == userEmail);
            if (user != null)
            {
                user.VerificationCode = verificationCode;
                _context.SaveChanges();
            }
            else
            {

                throw new InvalidOperationException("User not found");
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["MAIL_FROM_ADDRESS"]));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "Verification Code";
            email.Body = new TextPart(TextFormat.Html) { Text = $"Your verification code is: <b>{verificationCode}</b>" };

            using var smtp = new SmtpClient();
            smtp.Connect(_config["MAIL_HOST"], int.Parse(_config["MAIL_PORT"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["MAIL_USERNAME"], _config["MAIL_PASSWORD"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }


        
    }
}
