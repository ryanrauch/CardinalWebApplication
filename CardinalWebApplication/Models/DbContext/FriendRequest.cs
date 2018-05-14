using CardinalLibrary;
using System;

namespace CardinalWebApplication.Models.DbContext
{
    public class FriendRequest
    {
        public String InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }
        public String TargetId { get; set; }
        public ApplicationUser Target { get; set; }
        public DateTime TimeStamp { get; set; }
        public FriendRequestType? Type { get; set; }
    }
}
