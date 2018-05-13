using System.Collections.Generic;

namespace PolicyServer.EntityFramework.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public ICollection<PermissionRole> PermissionRoles { get; set; } = new List<PermissionRole>();
    }

    public class PermissionRole
    {
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
