using DemoIdentity.Services.MailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMailController : ControllerBase
    {
        private readonly IMailService mailService;

        public TestMailController(IMailService mailService)
        {
            this.mailService = mailService;
        }


        [HttpGet]
        public async Task<ActionResult> Test()
        {
           await mailService.SendMail("moh47567@gmail.com", "Test", "Hala Madried");
            return Ok();
        }
    }
}
