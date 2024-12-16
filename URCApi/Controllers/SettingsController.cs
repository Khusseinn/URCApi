using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URCApi.Data;
using URCApi.Entitites;

namespace URCApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {

        private readonly DataContext _context;

        private readonly IWebHostEnvironment _environment;

        public SettingsController(DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost("addSetting")]
        public async Task<IActionResult> AddSetting([FromBody] Setting setting)
        {
            if (setting == null || string.IsNullOrWhiteSpace(setting.Key))
            {
                return BadRequest("Setting məlumatları əlavə edin.");
            }

            _context.Settings.Update(setting);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Setting uğurla əlavə edildi.", Settings = setting });
        }

        [HttpDelete("/delete_setting_by/{id}")]
        public async Task<IActionResult> DeleteSetting(int id)
        {
            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSetting(int id, [FromBody] Setting setting)
        {
            if (id != setting.Id)
                return BadRequest("Id uyğun deyil");

            var existingSetting = await _context.Settings.FindAsync(id);

            if (existingSetting == null) 
                return NotFound($"Setting with Id = {id} not found");

            existingSetting.Key = setting.Key;
            existingSetting.Value = setting.Value;

            _context.Entry(existingSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingSetting);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Update zamanı xəta baş verdi: " + ex.Message);
            }
        }

        [HttpGet("/Key_Value")]
        public async Task<ActionResult<Dictionary<string, string>>> GetAll()
        {
            Dictionary<string, string> dictionary = await _context.Settings
                .ToDictionaryAsync(setting => setting.Key, setting => setting.Value);

            return Ok(dictionary);
        }
    }
}