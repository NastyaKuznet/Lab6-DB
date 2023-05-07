using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0
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
