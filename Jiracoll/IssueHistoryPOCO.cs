using Atlassian.Jira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiracoll
{
    class IssueHistoryPOCO
    {
        public string id { get; set; }
        //public string author { get; set; }
        public DateTime created { get; set; }
        public IList<IssueChangeLogItem> items { get; set; }




    }
}
