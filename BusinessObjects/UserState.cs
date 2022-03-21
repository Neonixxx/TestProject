using System.Collections.Generic;

namespace BusinessObjects
{
    public class UserState
    {
        public int UserStateId { get; set; }
        public UserStateCode Code { get; set; }
        public string Description { get; set; }
        
        public ICollection<User> Users { get; set; }
    }
}