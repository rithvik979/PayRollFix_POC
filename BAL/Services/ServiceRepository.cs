using Payrollfix_poc.IRepository;
using System.Net.Mail;
using System.Text;

namespace Payrollfix_poc.Services
{
    public class ServiceRepository : IServicesRepository
    {
        public string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@!#$%^&*()";
            StringBuilder newPassword = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 6; i++)  // Generate an 6-character password
            {
                newPassword.Append(validChars[random.Next(validChars.Length)]);
            }

            return newPassword.ToString();
        }
        public void SendResetPasswordEmail(string email, string newPassword)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("g.rithvikreddy909@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Password Reset Request";
                mail.Body = $"Your new password is: {newPassword}";

                //MailBee.SmtpMail.Smtp.QuickSend("jdoe@domain.com", email , sub, "Message Body");

                SmtpClient smtpServer = new SmtpClient();

                smtpServer.Host = "smtp.gmail.com";
                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("g.rithvikreddy909@gmail.com", "yxai scky pzcx aech");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                // Handle exception (log it or display an error message)
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
        }
    }
}
