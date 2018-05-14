using System.Collections.Generic;

namespace CardinalWebApplication.Models
{
    public class Polygon
    {
        public IList<Position> Positions { get; set; }
        public IList<Position> Holes { get; set; }
        public object Tag { get; set; }
    }
}
