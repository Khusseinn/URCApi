using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using URCApi.Data;
using URCApi.Entitites;

namespace URCApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly IWebHostEnvironment _environment;

        public NewsController(DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost("add_news")]
        public async Task<IActionResult> AddNewsWithImage([FromForm] NewsFormDataDto dto)
        {
            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest("Şəkil yüklənmədi.");

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".heic" };
            var fileExtension = Path.GetExtension(dto.Image.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Yalnız PNG, JPG və HEIC formatlı fayllar qəbul edilir.");

            string uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var news = new News
            {
                NewsHeader = dto.NewsHeader,
                NewsText = dto.NewsText,
                FileName = fileName,
                FilePath = filePath
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            news.ImageId = news.Id;

            _context.News.Update(news);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Xəbər və şəkil uğurla əlavə edildi",
                NewsId = news.Id,
                ImageId = news.ImageId,
                ImagePath = news.FilePath
            });
        }

        [HttpGet("/get_image_by/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var imageEntity = await _context.News.FindAsync(id);
            if (imageEntity == null)
                return NotFound();

            var filePath = imageEntity.FilePath;
            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg");
        }

        [HttpGet("/get_all_news")]
        public async Task<ActionResult<List<News>>> GetAll()
        {
            var menu = await _context.News.ToListAsync();
            return Ok(menu);
        }

        [HttpDelete("/delete_by/{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound(); 
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync(); 

            return NoContent(); 
        }

        [HttpPut("/update_news")]
        public async Task<IActionResult> UpdateNewsWithImage([FromForm] NewsUpdateFormDataDto dto)
        {
            var news = await _context.News.FindAsync(dto.Id);
            if (news == null)
                return NotFound("Xəbər tapılmadı.");

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".heic" };
                var fileExtension = Path.GetExtension(dto.Image.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                    return BadRequest("Yalnız PNG, JPG və HEIC formatlı fayllar qəbul edilir.");

                string uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                if (!string.IsNullOrEmpty(news.FilePath) && System.IO.File.Exists(news.FilePath))
                {
                    System.IO.File.Delete(news.FilePath);
                }

                var fileName = Guid.NewGuid() + fileExtension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                news.FileName = fileName;
                news.FilePath = filePath;
            }

            news.NewsHeader = dto.NewsHeader ?? news.NewsHeader;
            news.NewsText = dto.NewsText ?? news.NewsText;

            _context.News.Update(news);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Xəbər uğurla yeniləndi",
                NewsId = news.Id,
                ImagePath = news.FilePath
            });
        }
    }

    public class DocDto
        {
            public IFormFile Doc { get; set; }
        }
    }

    public class NewsFormDataDto
    {
        public string NewsHeader { get; set; }
        public string NewsText { get; set; }
        public IFormFile Image { get; set; }
    }

    public class NewsUpdateFormDataDto  
    {
        public int Id { get; set; } 
        public string NewsHeader { get; set; } 
        public string NewsText { get; set; } 
        public IFormFile Image { get; set; } 
    }

