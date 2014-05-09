using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace Map.Models
{
    public class LSD
    {
        public int Id { get; set; }
        public string PPID { get; set; }
        public string EFFDT { get; set; }
        public string FeatureCD { get; set; }
        public string Name { get; set; }
        public string QLSD { get; set; }
        public string PSECT { get; set; }
        public string PTWP { get; set; }
        public string PRGE { get; set; }
        public string PMER { get; set; }
        public string PQPPID { get; set; }
        public string LLD { get; set; }
        public SqlGeography Coordinates { get; set; }
    }
}
