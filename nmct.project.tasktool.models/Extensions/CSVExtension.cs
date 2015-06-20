using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Interfaces;
using nmct.project.tasktool.models.Properties.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models.Extensions
{
    public static class CSVExtension
    {
        public const string SEPERATOR = ";";
        public const string FILENAME_SEPERATOR = "-";

        public static string GetFilename(this ReportsLists report)
        {
            switch (report)
            {
                case ReportsLists.Runtime: return Model.Runtime;
                case ReportsLists.Performance: return Model.Performance;
                default: throw new ArgumentOutOfRangeException("No DisplayName Found!");
            }
        }

        public static string ToCsv<T>(this IEnumerable<T> values, string separator = SEPERATOR) where T : class, IHeaderline
        {
            //PropertyInfo[] properties = typeof(T).GetProperties();
            //string headerline = String.Join(separator,
            //    properties.Select(p => p.Name).Union(properties.Select(p => p.Name).ToArray())) + Environment.NewLine;

            List<string> list = new List<string>();
            string headerline = (values.First() as T).Headerline();
            list.Add(headerline);
            List<string> listValues = Array.ConvertAll(values.ToArray(), v => v.ToString()).ToList<string>();
            listValues.ForEach(v => list.Add(String.Join(separator, v).Remove(0, 1)));
            return String.Join(String.Empty, list);
        }
    }
}
