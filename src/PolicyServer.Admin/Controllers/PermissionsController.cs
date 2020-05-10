using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyServer.EntityFramework.DbContexts;
using PolicyServer.EntityFramework.Entities;

namespace PolicyServer.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly ConfigurationDbContext _context;

        public PermissionsController(ConfigurationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _context.Permissions
                .Include(x => x.PermissionRoles)
                    .ThenInclude(x => x.Role)
                .Select(x => new
                {
                    x.Name,
                    Roles = x.PermissionRoles.Select(y => y.Role.Name)
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(permissions);
        }

        [HttpGet("{permissionName}")]
        public async Task<IActionResult> GetPermission(string permissionName)
        {
            var permission = await _context.Permissions
                .Where(x => x.Name == permissionName)
                .Include(x => x.PermissionRoles)
                    .ThenInclude(x => x.Role)
                .Select(x => new
                {
                    x.Name,
                    Roles = x.PermissionRoles.Select(y => y.Role.Name)
                })
                .SingleOrDefaultAsync();

            if (permission == null)
                return NotFound();

            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> PostPermission([FromBody] PermissionEntity permission)
        {
            if (await _context.Permissions.AnyAsync(x => x.Name == permission.Name))
                return BadRequest("Invalid permission name");

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            return Created($"/permissions/{permission.Name}", new { permission.Name });
        }

        [HttpDelete("{permissionName}")]
        public async Task<IActionResult> DeletePermission(string permissionName)
        {
            var permission = await _context.Permissions.SingleOrDefaultAsync(x => x.Name == permissionName);
            if (permission == null)
                return NotFound();

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}