using System;

namespace CardinalWebApplication.Models.DbContext
{
    public class CurrentLayer
    {
        public ApplicationUser User { get; set; }
        public String UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public String LayersDelimited { get; set; }
        public Zone CurrentZone { get; set; }
        public String CurrentZoneId { get; set; }
    }
}
