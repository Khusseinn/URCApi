using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using URCApi.Data;
using URCApi.Entitites;


namespace URCApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeaderController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly IWebHostEnvironment _environment;
        public HeaderController(DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet("menu_bar")]
        public async Task<ActionResult<List<Menu>>> GetAll()
        {
            var menu = await _context.MenuBar.ToListAsync();
            return Ok(menu);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMenu([FromBody] Menu menu)
        {
            if (menu == null || string.IsNullOrWhiteSpace(menu.name))
            {
                return BadRequest("Menu məlumatları əlavə edin.");
            }

            _context.MenuBar.Update(menu);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Menu uğurla əlavə edildi.", Menu = menu });
        }

        [HttpDelete("/delete_menuitem_by/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menu = await _context.MenuBar.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            _context.MenuBar.Remove(menu);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}