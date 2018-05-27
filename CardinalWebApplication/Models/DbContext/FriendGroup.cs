using System;
using System.ComponentModel.DataAnnotations;

namespace CardinalWebApplication.Models.DbContext
{
    public class FriendGroup
    {
        [Key]
        public Guid ID { get; set; }
        public ApplicationUser User { get; set; }
        public string UserID { get; set; }
        public string Description { get; set; }
    }
}
