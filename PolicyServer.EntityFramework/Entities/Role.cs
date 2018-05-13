using System.Collections.Generic;

namespace PolicyServer.EntityFramework.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public ICollection<RoleSubject> RoleSubjects { get; set; } = new List<RoleSubject>();
    }

    public class RoleSubject
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public string Subject { get; set; }
    }
}