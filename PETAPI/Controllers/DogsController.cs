using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PETAPI.Data;
using PETAPI.Models;
using System.Linq;

namespace PETAPI.Controllers
{
    [ApiController]
    [Route("dogs")]
    public class DogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dog>>> GetDogs(string attribute = "name", string order = "asc", int pageNumber = 1, int pageSize = 10)
        {
            IQueryable<Dog> query = _context.Dogs;

            // Sorting
            switch (attribute.ToLower())
            {
                case "color":
                    query = order.ToLower() == "desc" ? query.OrderByDescending(d => d.Color) : query.OrderBy(d => d.Color);
                    break;
                case "tail_length":
                    query = order.ToLower() == "desc" ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength);
                    break;
                case "weight":
                    query = order.ToLower() == "desc" ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight);
                    break;
                default:
                    query = order.ToLower() == "desc" ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name);
                    break;
            }

            // Pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
