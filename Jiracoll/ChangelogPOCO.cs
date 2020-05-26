using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiracoll
{
    class ChangelogPOCO
    {
        public string startAt { get; set; }
        public string maxResults { get; set; }
        public string total { get; set; }
        public IList<IssueHistoryPOCO> histories { get; set; }

    }
}
