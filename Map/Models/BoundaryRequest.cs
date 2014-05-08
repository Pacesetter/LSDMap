using System;
using System.Linq;
using System.Collections.Generic;

namespace Map.Models
{
    public class BoundaryRequest
    {
        public int ZoomLevel { get; set; }
        public Coordinate NorthWest { get; set; }
        public Coordinate NorthEast { get; set; }
        public Coordinate SouthWest { get; set; }
        public Coordinate SouthEast { get; set; }
    }
}
