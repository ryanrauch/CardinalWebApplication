using System;
using System.ComponentModel.DataAnnotations;

namespace CardinalWebApplication.Models.DbContext
{
    public class FriendGroupUser
    {
        [Key]
        public Guid ID { get; set; }
        public FriendGroup Group { get; set; }
        public Guid GroupID { get; set; }
        public ApplicationUser Friend { get; set; }
        public string FriendID { get; set; }
    }
}
