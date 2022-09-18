using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class CustomTask
    {
        string Name { get; set; }
        string Duration { get; set; }
        DateTime Start { get; set; }
        DateTime Finish { get; set; }
        string Predecessors { get; set; }
        int OutlineLevel { get;set; }
        
    }
}
