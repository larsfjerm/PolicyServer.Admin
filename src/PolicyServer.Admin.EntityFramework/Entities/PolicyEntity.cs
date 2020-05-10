using System.Collections.Generic;

namespace PolicyServer.EntityFramework.Entities
{
    public class PolicyEntity
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public ICollection<PolicyRoleEntity> PolicyRoles { get; set; } = new List<PolicyRoleEntity>();
        public ICollection<PolicyPermissionEntity> PolicyPermissions { get; set; } = new List<PolicyPermissionEntity>();
    }

    public class PolicyRoleEntity
    {
        public int PolicyId { get; set; }
        public PolicyEntity Policy { get; set; }

        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }

    public class PolicyPermissionEntity
    {
        public int PolicyId { get; set; }
        public PolicyEntity Policy { get; set; }

        public int PermissionId { get; set; }
        public PermissionEntity Permission { get; set; }
    }
}