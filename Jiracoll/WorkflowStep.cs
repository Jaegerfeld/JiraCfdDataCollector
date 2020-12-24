using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiracoll
{
    class WorkflowStep
    {
        string name;
        string mapTarget;
        Boolean first = false;
        Boolean last = false;


        public WorkflowStep() { }
        public WorkflowStep(string name, string mapTarget)
        {
            this.name = name;
            this.mapTarget = mapTarget;
        }

        public string Name { get => name; set => name = value; }
        public string MapTarget { get => mapTarget; set => mapTarget = value; }
        public bool First { get => first; set => first = value; }
        public bool Last { get => last; set => last = value; }
    }
}
