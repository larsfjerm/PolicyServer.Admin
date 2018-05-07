using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyServer.EntityFramework.DbContexts;

namespace PolicyServer.Host.Controllers
{
    [Authorize]
    [Route("roles/{roleName}/subjects")]
    public class RoleSubjectsController : ControllerBase
    {
        private readonly ConfigurationDbContext _context;

        public RoleSubjectsController(ConfigurationDbContext context)
        {
            _context = context;
        }

        [HttpPost("{subject}")]
        public async Task<IActionResult> PostSubject(string roleName, string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                return BadRequest("No subject");

            var role = await _context.Roles.SingleOrDefaultAsync(x => x.Name != roleName);
            if (role == null)
                return NotFound();

            role.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return Created($"/roles/{role.Name}/subjects", subject);
        }

        [HttpDelete("{subject")]
        public async Task<IActionResult> DeleteSubject(string roleName, string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                return BadRequest("No subject");

            var role = await _context.Roles.SingleOrDefaultAsync(x => x.Name == roleName);
            if (role == null)
                return NotFound();

            role.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}