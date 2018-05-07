using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyServer.EntityFramework.DbContexts;
using PolicyServer.EntityFramework.Entities;

namespace PolicyServer.Host.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly ConfigurationDbContext _context;

        public RolesController(ConfigurationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles
                .Include(x => x.Subjects)
                .Select(x => new
                {
                    x.Name,
                    SubjectsCount = x.Subjects.Count
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(roles);
        }

        [HttpGet("{roleName}")]
        public async Task<IActionResult> GetRole(string roleName)
        {
            var role = await _context.Roles
                .Where(x => x.Name == roleName)
                .Include(x => x.Subjects)
                .Select(x => new
                {
                    x.Name,
                    x.Subjects
                })
                .SingleOrDefaultAsync();

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> PostRole([FromBody] Role role)
        {
            if (await _context.Roles.AnyAsync(x => x.Name == role.Name))
                return BadRequest("Invalid role name");

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Created($"/roles/{role.Name}", new { role.Name });
        }

        [HttpDelete("{roleName")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(x => x.Name == roleName);
            if (role == null)
                return NotFound();

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}