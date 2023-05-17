using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0
{
    public class AdditionalDataObject
    {
        public string WayCSV { get; set; }
        public ObjectDB[] Objects { get; set; }
        public AdditionalDataObject(string wayCSV, ObjectDB[] objectDB)
        {
            WayCSV = wayCSV;
            Objects = objectDB;
        }
        public AdditionalDataObject() { }
    }
}
