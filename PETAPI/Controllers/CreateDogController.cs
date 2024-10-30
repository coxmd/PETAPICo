using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PETAPI.Data;
using PETAPI.Models;

namespace PETAPI.Controllers
{
    [ApiController]
    [Route("dog")]
    public class CreateDogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CreateDogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Dog>> CreateDog(Dog dog)
        {
            // Check if dog with the same name already exists
            if (await _context.Dogs.AnyAsync(d => d.Name == dog.Name))
            {
                return BadRequest("Dog with the same name already exists.");
            }

            // Check if tail length is a negative number
            if (dog.TailLength < 0)
            {
                return BadRequest("Tail length cannot be a negative number.");
            }

            // Add the new dog to the database
            _context.Dogs.Add(dog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateDog), new { id = dog.Id }, dog);
        }
    }
}
