using FileUploadPractice.Data;
using FileUploadPractice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _environment;

        public DocumentController(ApplicationDbContext context,IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            long size=file.Length;
            var rootPath = Path.Combine(_environment.ContentRootPath, "Resourses", "Document");
            if(!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var filePath=Path.Combine(rootPath, file.FileName);
            using(var stream=new FileStream(filePath, FileMode.Create))
            {
                var document = new Document
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Size = file.Length
                };
                await file.CopyToAsync(stream);
                _context.Document.Add(document);
                await _context.SaveChangesAsync();
            }

            return Ok(file.Length);
        }
    }
}
