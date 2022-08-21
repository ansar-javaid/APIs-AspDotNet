using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPattrenPractice.Models;
using RepositoryPattrenPractice.Repository;

namespace RepositoryPattrenPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly Interface _interface;

        public StudentsController(Interface @interface)
        {
            _interface = @interface;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            return Ok(await _interface.GetAll());
        }

        [HttpPost]
        public async Task<IEnumerable<Student>> Creat(Student student)
        {
            await _interface.Creat(student);
            return await _interface.GetAll();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Student>>> Search(string name)
        {
            if(await _interface.search(name) == null)
                return NotFound("Record not found");
            return Ok(await _interface.search(name));
        }
    }
}
