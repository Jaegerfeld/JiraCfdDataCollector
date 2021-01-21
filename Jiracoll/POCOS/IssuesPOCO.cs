using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiracoll
{
    class IssuesPOCO
    {
        public string expand { get; set; }
        public string startAt { get; set; }
        public string maxResults { get; set; }
        public string total { get; set; }
        public IList<IssuePOCO> issues { get; set; }
    }
}
