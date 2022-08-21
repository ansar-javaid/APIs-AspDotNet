using System.ComponentModel.DataAnnotations;

namespace RepositoryPattrenPractice.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

    }
}
