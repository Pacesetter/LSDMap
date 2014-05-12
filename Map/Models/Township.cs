using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace Map.Models
{
    public class Township
    {
        public int Id { get; set; }
        public string PPID { get; set; }
        public string EFFDT { get; set; }
        public string FeatureCD { get; set; }
        public string TWP { get; set; }
        public string RGE { get; set; }
        public string MER { get; set; }
        public string TRM { get; set; }
        public string Name
        {
            get
            {
                return String.Format("{0}-{1} W{2}", TWP, RGE, MER);
            }
        }
        public SqlGeography Coordinates { get; set; }
    }
}
