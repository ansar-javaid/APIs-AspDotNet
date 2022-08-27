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
        /// <summary>
        /// A file upload api, that accepts a file and save that file in storage and save its details in Database
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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





        //============================================================================================
        /// <summary>
        /// Get the uploaded file from the stograge and download/return it to the user
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
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
        }//Get or Download Ended---------------------------------------------------------------------------




        //=================================================================================================
        /// <summary>
        /// Delete the Existing file from storage and its details from database
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        [HttpDelete("{fileID}")]
        public async Task<ActionResult> Delete(int fileID)
        {
            var document = await _context.Document.FindAsync(fileID);
            if (document == null)
                return NotFound();
            var file = Path.Combine(_environment.ContentRootPath, "Resourses", "Document", document.FileName);

            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
                _context.Document.Remove(document);
                await _context.SaveChangesAsync();
                return Ok("File deleted successfuly!");
            }
            return NotFound("Record not found!");
        }//Delete Endend--------------------------------------------------------------------------------------



        //====================================================================================================
        /// <summary>
        /// Replace the existing file with thw=e newly uploaded file, by deleteing the privious one
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="newfile"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<ActionResult> Update(int fileID,IFormFile newfile)
        {
            //Findind Record in DB
            var document = await _context.Document.FindAsync(fileID);
            if (document == null)
                return NotFound();
            //Finding File in Storage
            var file = Path.Combine(_environment.ContentRootPath, "Resourses", "Document", document.FileName);

            if (System.IO.File.Exists(file))
            {
                //Deleteing the Founded file and keepthe name save
                System.IO.File.Delete(file);
                long size = newfile.Length;
                //Finding path and creating the Directory if not exisist
                var rootPath = Path.Combine(_environment.ContentRootPath, "Resourses", "Document");
                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);
                string[] split = newfile.ContentType.Split('/');
                string[] split1=document.FileName.Split('.');
                var FileName = split1[0] + '.' + split[1];

                //Creating new file and assigning the previous file name to the new file
                var filePath = Path.Combine(rootPath, FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {    
                    //update the new file details in the existing database record
                    document.FileName=FileName;
                    document.ContentType=newfile.ContentType;
                    document.Size= size;
                    await newfile.CopyToAsync(stream);
                    await _context.SaveChangesAsync();
                }
                return Ok("File Replaced successfuly!");
            }
            return NotFound("Record not found!");
        }//Repace file or Update Ended--------------------------------------------------------------------

    }
}
