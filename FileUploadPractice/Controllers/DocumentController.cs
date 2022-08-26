using FileUploadPractice.Data;
using FileUploadPractice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Collections.Specialized;

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


        //================================================================================================
        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            long size=file.Length;
            var rootPath = Path.Combine(_environment.ContentRootPath, "Resourses", "Document");
            if(!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
            string[] split = file.ContentType.Split('/');
            var uniqueFileName = Guid.NewGuid().ToString() + '.' + split[1];

            var filePath = Path.Combine(rootPath, uniqueFileName);
            using(var stream=new FileStream(filePath, FileMode.Create))
            {
                var document = new Document
                {
                    FileName = uniqueFileName,
                    ContentType = file.ContentType,
                    Size = file.Length
                };
                await file.CopyToAsync(stream);
                _context.Document.Add(document);
                await _context.SaveChangesAsync();
            }

            return Ok(file.Length);
        }//Upload Ended-------------------------------------------------------------------------------

        [HttpGet]
        public async Task<ActionResult> Download(int fileID)
        {
            var provider=new FileExtensionContentTypeProvider();
            var document = await _context.Document.FindAsync(fileID);
            if(document == null)
                return NotFound();
            var file = Path.Combine(_environment.ContentRootPath, "Resourses", "Document", document.FileName);

            string contentType;
            if(!provider.TryGetContentType(file,out contentType))
            {
                contentType = "application/octet-stream";
            }

            byte[] fileBytes;
            if(System.IO.File.Exists(file))
            {
                fileBytes = System.IO.File.ReadAllBytes(file);
            }
            else
                return NotFound();
            return File(fileBytes,contentType,document.FileName);
        }
    }
}
