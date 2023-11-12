using Microsoft.AspNetCore.Http;


namespace SendEmail.BL.Dtos
{
    public class MailDto
    {

        public string? subject { get; set; }
        public string? body { get; set; }
        public string? receiver { get; set; }
        public string? receiverName { get; set; }
        public List<IFormFile> attachments { get; set; }
    }
}
