using CardinalLibrary;
using System;
using System.ComponentModel.DataAnnotations;

namespace CardinalWebApplication.Models.DbContext
{
    public class Zone
    {
        [Key]
        public Guid ZoneID { get; set; }
        public string Description { get; set; }
        public string VisibleToLayersDelimited { get; set; }
        public string ARGBFill { get; set; }
        public ZoneType Type { get; set; }
    }
}
