using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class CustomTask
    {
        internal string Name { get; set; }
        internal string Duration { get; set; }
        internal DateTime Start { get; set; }
        internal DateTime Finish { get;set; }
        internal int Predecessors { get; set; }
        internal int SubTasks { get; set; }
    }
}
