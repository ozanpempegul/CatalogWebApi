using CatalogWebApi.Base;
using CatalogWebApi.Dto;
using System.Net;
using System.Net.Mail;

namespace CatalogWebApi.Service
{
    public class Message
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(string to, string subject, string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }
    }

    public class EmailSender: IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        private MailMessage CreateEmailMessage(Message message)
        {
            MailMessage msgObj = new MailMessage();
            msgObj.To.Add(message.To);
            msgObj.From = new MailAddress(_emailConfig.From);
            msgObj.Subject = message.Subject;
            msgObj.Body = message.Content;            

            return msgObj;
        }

        private void Send(MailMessage mailMessage)
        {
            using (SmtpClient client = new(_emailConfig.SmtpServer, _emailConfig.Port))
            {
                try
                {
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;

                    // reading it from credentials.txt, password is app pasword not the actual password of the email.
                    client.Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    throw new MessageResultException("Error occured while sending an e-mail.");
                }
                finally
                {
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MailMessage mailMessage)
        {
            using (SmtpClient client = new(_emailConfig.SmtpServer, _emailConfig.Port))
            {
                try
                {
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;

                    // reading it from credentials.txt, password is app pasword not the actual password of the email.
                    client.Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password);
                    client.SendAsync(mailMessage, "succeed");
                }
                catch
                {
                    throw new MessageResultException("Error occured while sending an e-mail.");
                }
                finally
                {
                    client.Dispose();
                }
            }
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);
            await SendAsync(mailMessage);
        }

    }
}
