using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSynchronization
{
    class Process
    {
        public string name { get; set; }
        public int state { get; set; }
        public List<string> BlockedBy;

        public Process(string name, int state)
        {
            this.name = name;
            this.state = state;
            BlockedBy = new List<string>();
        }
        
    }
}
