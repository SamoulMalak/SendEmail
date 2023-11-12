using Microsoft.AspNetCore.Mvc;
using SendEmail.BL.Dtos;
using SendEmail.BL.IServices;

namespace SendEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailServices mail;

        public MailController(IMailServices mail)
        {
            this.mail = mail;
        }
        [HttpPost]
        public async Task<IActionResult> SendMSgAsync([FromForm] MailDto obj)
        {
            var result = await mail.SendEmailAsync(obj.subject, obj.body, obj.receiver, obj.receiverName, obj.attachments);
            if (result)
            {
                return Ok("Email Sent successfully");
            }
            return BadRequest("The Email was not sent successfully");
        }
    }
}
