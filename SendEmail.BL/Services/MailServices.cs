using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendEmail.BL.IServices;

namespace SendEmail.BL.Services
{
    public class MailServices :IMailServices
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _host;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _sender;
        public MailServices(IConfiguration config)
        {
            _smtpClient = new SmtpClient();
            var section = config.GetSection("EmailSettings");
    

            _host = section["Host"];
            _port = int.Parse(section["Port"]);
            _userName = section["Username"];
            _password = section["Password"];
            _sender = section["Sender"];
        }
        public async Task<bool> SendEmailAsync(string subject, string body, string receiver, string receiverName, List<IFormFile> attachments)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Samoul_MailServices", _sender));
                email.To.Add(new MailboxAddress(receiverName, receiver));
                email.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = body;

                foreach (var attachment in attachments)
                {
                    var memoryStream = new MemoryStream();
                    await attachment.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var attachmentContent = new MimeContent(memoryStream);

                    var attachmentPart = new MimePart()
                    {
                        Content = attachmentContent,
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = attachment.FileName
                    };

                    bodyBuilder.Attachments.Add(attachmentPart);
                }
               

                email.Body = bodyBuilder.ToMessageBody();

                await _smtpClient.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);

                await _smtpClient.AuthenticateAsync(_userName, _password);

                await _smtpClient.SendAsync(email);
                _smtpClient.Disconnect(true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
