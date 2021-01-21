using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiracoll
{
    class StatusRich
    {
        string name;
        int minutes;
        DateTime timeStamp;

        public string Name { get => name; set => name = value; }
        public int Minutes { get => minutes; set => minutes = value; }
        public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }

        public StatusRich() { }
        public StatusRich( string name, DateTime transformationDate)
        {
            this.name = name;
            this.timeStamp = transformationDate;
            this.minutes = 0;
        }

    }
}
