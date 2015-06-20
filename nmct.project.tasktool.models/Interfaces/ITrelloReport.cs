using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmct.project.tasktool.models.Interfaces
{
    public interface ITrelloReport
    {
        string Name { get; set; }
        double Time { get; set; }
        DateTime CreationTime { get; set; }
    }
}
