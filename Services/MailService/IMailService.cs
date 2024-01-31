using Microsoft.AspNetCore.Http;

namespace DemoIdentity.Services.MailService
{
    public interface IMailService
    {
        public Task<bool> SendMail(string MailTo, string Subject, string Body, IList<IFormFile> Attachments = null);
    }
}
