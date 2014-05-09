using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Map.Models
{
    public class Box
    {
        public int id { get; set; }
        public string Name { get; set; }
        public SqlGeography Coordinates { get; set; }
    }
}