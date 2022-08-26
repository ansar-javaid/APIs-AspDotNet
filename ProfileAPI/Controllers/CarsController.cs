using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileAPI.Data;
using ProfileAPI.Models;

namespace ProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        // Databse Contect Class Connection
        private readonly ApplicationDbContext _context;

        //Constructor---------------------------------------------------------------------------------
        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }
        //=============================================================================================

        [HttpGet]
        public async Task<ActionResult<List<Car>>> GetAllCars()
        
        {  
            return Ok(await _context.Car.ToListAsync());
        }

        //===========================================================================================================

        [HttpPost]
        public async Task<ActionResult<List<Car>>> InsertCar(Car car)
        {
            _context.Car.Add(car);
            await _context.SaveChangesAsync();
            return Ok(await _context.Car.ToListAsync());
        }
        //===========================================================================================================




        [HttpGet("{id}")]
        public async Task<ActionResult<List<Car>>> GetSpecificCar(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car == null)
                return BadRequest("Record Not Found");
            return Ok(car);
        }
        //===========================================================================================================


        [HttpPut]
        public async Task<ActionResult<List<Car>>> UpdateCar(Car requestCar)
        {
            var car = await _context.Car.FindAsync(requestCar.Id); 
            if (car == null)
                return BadRequest("Record Not found");
            car.Name=requestCar.Name;
            car.Colour=requestCar.Colour;
            await _context.SaveChangesAsync();
            return Ok(await _context.Car.ToListAsync());
        }
        //===========================================================================================================


        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Car>>> DeleteCar(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car == null)
                return NotFound("Chez nhi mili");
            _context.Car.Remove(car);
            await _context.SaveChangesAsync();  
            return Ok(await _context.Car.ToListAsync());
        }
        //===========================================================================================================







    }
}
