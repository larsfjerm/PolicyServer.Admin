using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyServer.EntityFramework.DbContexts;

namespace PolicyServer.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly ConfigurationDbContext _context;

        public PoliciesController(ConfigurationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hei");
        }

        [HttpGet("{policyName}")]
        public async Task<IActionResult> GetPolicy(string policyName, [FromQuery] string secret)
        {
            if (string.IsNullOrWhiteSpace(policyName))
                return BadRequest();
            if (string.IsNullOrWhiteSpace(secret))
                return BadRequest("Query attribute policySecret is missing");

            switch (policyName)
            {
                case "Hospital":
                    if(secret.Equals("secret"))
                        break;
                    return Unauthorized();
                default:
                    return Unauthorized();
            }

            var policy = await _context.Policies
                .Include(x => x.PolicyRoles)
                    .ThenInclude(x => x.Role)
                        .ThenInclude(x => x.RoleSubjects)
                .Include(x => x.PolicyPermissions)
                    .ThenInclude(x => x.Permission)
                        .ThenInclude(x => x.PermissionRoles)
                            .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(x => x.Name == policyName);

            if (policy == null)
                return NotFound();

            var policyResult = new
            {
                Roles = policy.PolicyRoles.Select(x => new
                {
                    x.Role.Name,
                    Subjects = x.Role.RoleSubjects.Select(y => y.Subject)
                }),
                Permissions = policy.PolicyPermissions.Select(x => new
                {
                    x.Permission.Name,
                    Roles = x.Permission.PermissionRoles.Select(y => y.Role.Name)
                })
            };

            return Ok(policyResult);
        }
    }
}