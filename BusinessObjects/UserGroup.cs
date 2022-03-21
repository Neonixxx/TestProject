using System.Collections.Generic;

namespace BusinessObjects
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public UserGroupCode Code { get; set; }
        public string Description { get; set; }
        
        public ICollection<User> Users { get; set; }
    }
}