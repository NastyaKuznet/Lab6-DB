using Core0;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0
{
    public class ElementDB
    {
        public AdditionalDataPattern FullPattern { get; set; }
        public AdditionalDataObject Data { get; set; }
        public DataTable Table { get; set; }
        public bool IsNeedUpdate { get; set; }
    }
}
