using System.Collections.Generic;

namespace PolicyServer.EntityFramework.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public ICollection<RoleSubjectEntity> RoleSubjects { get; set; } = new List<RoleSubjectEntity>();
    }

    public class RoleSubjectEntity
    {
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        public string Subject { get; set; }
    }
}