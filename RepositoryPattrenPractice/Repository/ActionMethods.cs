using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepositoryPattrenPractice.Data;
using RepositoryPattrenPractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace RepositoryPattrenPractice.Repository
{
    public class ActionMethods : Interface
    {
        //Injecting Db Service
        private readonly ApplicationDbContext _context;

        //Constructor---------------------------------------
        public ActionMethods(ApplicationDbContext context)
        {
            this._context = context;
        }


        public async Task<IEnumerable<Student>> Creat(Student student)
        {
            _context.Student.Add(student);
            await _context.SaveChangesAsync();
            return await _context.Student.ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Student.ToListAsync();
        }

        public async Task<IEnumerable<Student>> search(string name)
        {
            //Building a Search Query from DB
            IQueryable<Student> searchQuery=_context.Student;

            if(!string.IsNullOrEmpty(name))
            {
                searchQuery = searchQuery.Where(h => h.Name.Equals(name));
            }
            return await searchQuery.ToListAsync();
        }
    }
}
