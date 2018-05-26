using System;
using System.ComponentModel.DataAnnotations;

namespace CardinalWebApplication.Models.DbContext
{
    public class ZoneShape
    {
        [Key]
        public Guid ZoneShapeID {get;set;}
        public Guid ParentZoneId { get; set; }
        public Zone ParentZone { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
