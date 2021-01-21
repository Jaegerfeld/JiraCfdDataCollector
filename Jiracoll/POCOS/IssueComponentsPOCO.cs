using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlassian.Jira;

namespace Jiracoll
{
    class IssueComponentsPOCO
    {
        public LinkedList<IssueComponentsItemPOCO> items { get; set; }

    }
}
