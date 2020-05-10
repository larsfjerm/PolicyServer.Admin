using System.Collections.Generic;

namespace PolicyServer.EntityFramework.Entities
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public ICollection<PermissionRoleEntity> PermissionRoles { get; set; } = new List<PermissionRoleEntity>();
    }

    public class PermissionRoleEntity
    {
        public int PermissionId { get; set; }
        public PermissionEntity Permission { get; set; }

        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }
}
