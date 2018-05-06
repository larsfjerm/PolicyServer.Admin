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

        public List<Role> Roles { get; set; }
    }
}
