using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net.Mime;

namespace SendEmail.BL.IServices
{
    public interface IMailServices
    {
        Task<bool> SendEmailAsync(string subject, string body, string receiver, string receiverName, List<IFormFile> attachments);

    }
   
}
