using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileAPI.Models;

namespace ProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        //Sample hard coded/static data
        private static List<Profile> profiles = new List<Profile>
        {
            new Profile
            {
                Id = 1,
                Name="Thor",
                Age=1050
            },
            new Profile
            {
                Id = 2,
                Name="Iron Man",
                Age=55
            },
            new Profile
            {
                Id = 3,
                Name="Hulk",
                Age=100
            }
        };


        //Get Everything
        [HttpGet]
        public async Task<ActionResult<List<Profile>>> Get() //
        {
            return Ok(profiles);
        }//Get Ended


        //Get Specific 
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetSpecific(int id)
        {
            var profile = profiles.Find(h =>h.Id == id);
            if (profile == null)
                return BadRequest("Record not found");
            return Ok(profile);
        }//Get Specific Ended


        // Post/Add somthing in data
        [HttpPost]
        public async Task<ActionResult<List<Profile>>> AddProfiles(Profile profile)
        {
            profiles.Add(profile);
            return Ok(profiles);
        }//Post ended


        // Put/Replace the existing data
        [HttpPut]
        public async Task<ActionResult<List<Profile>>> UpdateProfiles(Profile profileRequest)
        {
            var profile = profiles.Find(h => h.Id == profileRequest.Id);
            if (profile == null)
                return BadRequest("Record not found.");
            //Updates Data
            profile.Name = profileRequest.Name;
            profile.Age = profileRequest.Age;
            return Ok(profiles);
        }//Put Ended


        // Delete something from existing data
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Profile>>> Delete(int id)
        {
            var profile = profiles.Find(h => h.Id == id);
            if (profile == null)
                return BadRequest("Record not found");
            //Removing
            profiles.Remove(profile);
            return Ok(profiles);
        }//Delete Ended
    }
}
