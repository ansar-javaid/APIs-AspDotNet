using System.ComponentModel.DataAnnotations;

namespace FileUploadPractice.Models
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string ContentType { get; set; }
        public long? Size { get; set; }
    }
}
