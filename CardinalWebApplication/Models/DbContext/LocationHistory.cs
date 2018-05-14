using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardinalWebApplication.Models.DbContext
{
    public class LocationHistory
    {
        [Key]
        public Guid HistoryID { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
