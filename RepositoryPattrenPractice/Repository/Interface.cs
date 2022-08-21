using Microsoft.AspNetCore.Mvc;
using RepositoryPattrenPractice.Models;

namespace RepositoryPattrenPractice.Repository
{
    public interface Interface
    {
        public Task<IEnumerable<Student>> Creat(Student student);

        public Task<IEnumerable<Student>> GetAll();

        public Task<IEnumerable<Student>> search(string name);

    }
}
