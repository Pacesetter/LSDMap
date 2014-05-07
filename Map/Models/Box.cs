using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Map.Models
{
    public class Box
    {
        int id { get; set; }
        public string Name { get; set; }
        IEnumerable<Coordinate> Coordinates { get; set; }
    }
}
