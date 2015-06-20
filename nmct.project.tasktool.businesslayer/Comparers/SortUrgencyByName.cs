using nmct.project.tasktool.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.businesslayer.Comparers
{
    public class SortUrgencyByName : IComparer<Urgency>
    {
        public int Compare(Urgency x, Urgency y)
        {
            if (y.Name.CompareTo(x.Name) > 1)
                return 1;
            else if (y.Name.CompareTo(x.Name) < -1)
                return -1;
            else
                return 0;
        }
    }
}
