﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiracoll
{
    class IssueFieldsPOCO
    {
        public string id { get; set; }
    
        public DateTime created { get; set; }
        public IssueTypePOCO issuetype { get; set; }
        public IssueStatusPOCO status { get; set; }

    }
}
