using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URCApi.Data;
using URCApi.Entitites;
using URCApi.Migrations;
using URCApi.Services;
using System.Threading.Tasks;
using Contact = URCApi.Entitites.Contact;

namespace URCApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Contacts : ControllerBase
    {
        private readonly DataContext _context;
        private readonly EmailService _emailService;
        public Contacts()
        {
            _emailService = new EmailService();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendContact([FromBody] Entitites.Contact contact)
        {
            if (contact == null || string.IsNullOrWhiteSpace(contact.Mail) || string.IsNullOrWhiteSpace(contact.Subject))
            {
                return BadRequest("Məlumatlar düzgün deyil!");
            }

            try
            {
                await _emailService.SendContactEmailAsync(contact);
                return Ok("Email uğurla göndərildi.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Email göndərilərkən xəta baş verdi: {ex.Message}");
            }
        }
    }
}