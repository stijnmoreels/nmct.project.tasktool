using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Extensions;
using nmct.project.tasktool.models.Interfaces;
using nmct.project.tasktool.models.Properties.TaskPerformance;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models
{
    public class TaskPerformance : IHeaderline, IReportName
    {
        [Key]
        public DateTime Date { get; set; }
        public int TasksDone { get; set; }
        public int TasksInProgress { get; set; }
        public int TasksToDo { get; set; }

        public int TrelloCampusId { get; set; }
        public virtual TrelloCampus TrelloCampus { get; set; }

        private int _totalTasks;
        [NotMapped]
        public int TotalTasks
        {
            get { return this.TasksDone + this.TasksInProgress + this.TasksToDo; }
            set { _totalTasks = value; }
        }

        public override string ToString()
        {
            return this.Date + CSVExtension.SEPERATOR +
                this.TasksDone + CSVExtension.SEPERATOR +
                this.TasksInProgress + CSVExtension.SEPERATOR +
                this.TasksToDo + CSVExtension.SEPERATOR +
                this.TrelloCampusId + CSVExtension.SEPERATOR +
                Environment.NewLine;
        }

        public string Headerline()
        {
            return String.Join(CSVExtension.SEPERATOR, Model.Date, Model.TasksDone, 
                Model.TasksInProgress, Model.TasksToDo, Environment.NewLine);
        }

        [NotMapped]
        public ReportsLists Report { get; set; }

        public TaskPerformance()
        {
            this.Report = ReportsLists.Performance;
        }
    }
}
