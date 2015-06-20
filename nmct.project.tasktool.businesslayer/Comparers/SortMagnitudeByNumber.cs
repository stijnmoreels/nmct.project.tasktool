using nmct.project.tasktool.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.businesslayer.Comparers
{
    public class SortMagnitudeByNumber : IComparer<Magnitude>
    {
        public int Compare(Magnitude x, Magnitude y)
        {
            if (x.Number > y.Number)
                return 1;
            else if (x.Number < y.Number)
                return -1;
            else
                return 0;
        }
    }
}
