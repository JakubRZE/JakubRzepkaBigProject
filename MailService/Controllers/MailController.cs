using System;
using System.Threading.Tasks;
using MailService.Interfaces;
using MailService.Model;
using Microsoft.AspNetCore.Mvc;


namespace MailService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailSrv _mailSrv;

        public MailController(IMailSrv mailSrv)
        {
            _mailSrv = mailSrv;
        }

        /// <summary>
        /// Get All Mails by userId as parameter.
        /// </summary>
        /// <param name="id">Id of User.</param>
        [HttpGet("User/{id}")]
        public async Task<IActionResult> GatAllMailsAsync(int id)
        {
            var mails = await _mailSrv.GatAllMailsByUserIdAsync(id);
            return Ok(mails);
        }

        /// <summary>
        /// Get Mail by mailId  as parameter.
        /// </summary>
        /// <param name="id">Id of Mail.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GatMailByIdAsync(int id)
        {
            var mail = await _mailSrv.GatMailByIdAsync(id);
            if (mail == null) return NotFound();
            return Ok(mail);
        }


        /// <summary>
        /// Get Mail status by mailId as parameter.
        /// </summary>
        /// <param name="id">Id of Mail.</param>
        [HttpGet("Status/{id}")]
        public async Task<IActionResult> GatMailStatusByIdAsync(int id)
        {
            try
            {
                var status = await _mailSrv.GatMailStatusByIdAsync(id);
                return Ok(status);
            }
            catch (InvalidOperationException)
            {
                return NotFound("Mail with requested Id not found");
            }
        }

        /// <summary>
        /// Save mail and returns status code.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        public async Task<IActionResult> SaveMailAsync([FromBody] MailDto model)
        {
            int id = await _mailSrv.SaveMailAsync(model);
            return Created(nameof(SaveMailAsync), id);
        }

        /// <summary>
        /// Send all pending mails.
        /// </summary>
        /// <param name="id">Id of User.</param>
        /// <returns></returns>
        [HttpPost("Send")]
        public async Task<IActionResult> Post(int id)
        {
            await _mailSrv.SendMailAsync(id);
            return Ok();
        }
    }
}
