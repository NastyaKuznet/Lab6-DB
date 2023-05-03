using Lab6DB.Model.PrimaryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6DB.Model.AdditionalData
{
    public class AdditionalDataPattern
    {
        public string WayJson { get; set; }
        public PatternObjectDB Pattern { get; set; }
        public AdditionalDataPattern(string way, PatternObjectDB pattern) 
        {
            WayJson = way;
            Pattern = pattern;
        }
        public AdditionalDataPattern() { }
    }
}
