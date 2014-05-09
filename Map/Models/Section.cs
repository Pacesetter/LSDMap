using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace Map.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string PPID { get; set; }
        public string EFFDT { get; set; }
        public string FeatureCD { get; set; }
        public string SECT { get; set; }
        public string PTWPPID { get; set; }
        public string PTWP { get; set; }
        public string PRGE { get; set; }
        public string PMER { get; set; }
        public string Name
        {
            get
            {
                return String.Format("{0}-{1}-{2}", PTWP, PRGE, PMER);
            }
        }

        public SqlGeography Coordinates { get; set; }
    }
}
