using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiracoll
{
    class IssuePOCO
    {

        public string expand { get; set; }
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public ChangelogPOCO changelog { get; set; }
        public IssueFieldsPOCO fields { get; set; }
       

    }
}
