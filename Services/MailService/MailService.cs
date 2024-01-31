using DemoIdentity.Setting;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
namespace DemoIdentity.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly MailSettings mailSetting;

        public MailService(IOptions<MailSettings> mailSetting)
        {
            this.mailSetting = mailSetting.Value;
        }
        public async Task<bool> SendMail(string MailTo, string Subject, string Body, IList<IFormFile> Attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailSetting.Email),
                Subject = Subject
            };

            email.To.Add(MailboxAddress.Parse(MailTo));

            var builder = new BodyBuilder();

            if (Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in Attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = Body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(mailSetting.DisplayName, mailSetting.Email));
             
          using var smtp = new SmtpClient();
            smtp.Connect(mailSetting.Host, mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSetting.Email, mailSetting.Password);
            var result = await smtp.SendAsync(email);
          
            smtp.Disconnect(true);
            var status = result.Split(" ")[0];
            if (status == "2.0.0")
                return true;
            else 
                return false;
        }
    }
}
