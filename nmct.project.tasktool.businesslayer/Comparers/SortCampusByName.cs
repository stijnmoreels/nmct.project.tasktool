using nmct.project.tasktool.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.businesslayer.Comparers
{
    public class SortCampusByName : IComparer<Campus>
    {
        public int Compare(Campus x, Campus y)
        {
            if (x.Name.CompareTo(y.Name) > 1)
                return 1;
            else if (x.Name.CompareTo(y.Name) < -1)
                return -1;
            else
                return 0;
        }
    }
}
