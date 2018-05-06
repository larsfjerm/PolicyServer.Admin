using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyServer.EntityFramework.DbContexts;

namespace PolicyServer.Host.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly ConfigurationDbContext _context;

        public PoliciesController(ConfigurationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetPolicy()
        {
            var policy = await _context.Policies
                .Include(x => x.Permissions)
                    .ThenInclude(x => x.Roles)
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Subjects)
                .SingleOrDefaultAsync(x => x.Id == 1);

            if (policy == null)
                return NotFound();

            var policyResult = new
            {
                Roles = policy.Roles.Select(x => new
                {
                    x.Name,
                    x.Subjects
                }),
                Permissions = policy.Permissions.Select(x => new
                {
                    x.Name,
                    x.Roles
                })
            };

            return Ok(policyResult);
        }
    }
}