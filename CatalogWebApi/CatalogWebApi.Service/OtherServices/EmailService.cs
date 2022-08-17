using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using System.Net;
using System.Net.Mail;

namespace CatalogWebApi.Service
{
    public class EmailService
    {
        public void SendEmail(AccountDto createAccountResource, string subject, string body)
        {

            string[] lines = File.ReadAllLines("credentials.txt");
            string email = lines[0];
            string password = lines[1];

            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                // reading it from credentials.txt, password is app pasword not the actual password of the email.
                client.Credentials = new NetworkCredential(email, password);

                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(createAccountResource.Email);
                msgObj.From = new MailAddress(email);
                msgObj.Subject = subject;
                msgObj.Body = body;
                client.Send(msgObj);
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Sending_Email_Failed", ex);
            }

        }

    }
}
