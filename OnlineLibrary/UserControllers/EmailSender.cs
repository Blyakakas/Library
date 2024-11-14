using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary
{
    internal class EmailSender
    {
        private static string GenerateVerificationCode()
        {
            Random random = new Random();
            int code = random.Next(100000, 1000000);
            return code.ToString();
        }

        public static string SendVerificationCode(string userEmail)
        {
            string verificationCode = GenerateVerificationCode();
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string senderEmail = "artem27082009a@gmail.com";
                string senderPassword = "fuzu vnbo yohz hacz";
                string subject = "ANUS";
                string body = $"FIT ANUS CODE: {verificationCode}";

                MailMessage mailMessage = new MailMessage(senderEmail, userEmail, subject, body);
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                };

                smtpClient.Send(mailMessage);
                Console.WriteLine($"Verification code sent to {userEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            return verificationCode;
        }
    }
}
