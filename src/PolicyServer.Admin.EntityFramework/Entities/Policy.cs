using System.Collections.Generic;

namespace PolicyServer.EntityFramework.Entities
{
    public class Policy
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public ICollection<PolicyRole> PolicyRoles { get; set; } = new List<PolicyRole>();
        public ICollection<PolicyPermission> PolicyPermissions { get; set; } = new List<PolicyPermission>();
    }

    public class PolicyRole
    {
        public int PolicyId { get; set; }
        public Policy Policy { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class PolicyPermission
    {
        public int PolicyId { get; set; }
        public Policy Policy { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}