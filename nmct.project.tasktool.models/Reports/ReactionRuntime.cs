using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Extensions;
using nmct.project.tasktool.models.Interfaces;
using nmct.project.tasktool.models.Properties.ReactionRuntime;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models.Reports
{
    public class ReactionRuntime : IReactionRuntime, IHeaderline, IReportName
    {
        public int Id { get; set; }

        public string TrelloCardId { get; set; }

        public DateTime? CardIsDone { get; set; }
        public DateTime CardOnProgress { get; set; }
        public DateTime CardCreated { get; set; }


        [NotMapped]   
        public String[] MemberIds { get; set; }


       // public List<string> TrelloMemberId { get; set; }
        //[ForeignKey("TrelloMemberId")]
        public List<TrelloMember> TrelloMembers { get; set; }



        public int TrelloCampusId { get; set; }
        public virtual TrelloCampus TrelloCampus { get; set; }



        // Add new Propertie "List<TrelloMember>" -> Multi-Reference
        public ReactionRuntime(string trelloCardId, DateTime cardOnProgress, DateTime cardCreated, int trelloCampusId, params TrelloMember[] members)
        {
            this.TrelloCardId = trelloCardId;
            this.CardOnProgress = CardOnProgress;
            this.CardCreated = cardCreated;
            this.TrelloCampusId = trelloCampusId;
            this.TrelloMembers = members.ToList<TrelloMember>();
        }

        //public ReactionRuntime(string trelloCardId, DateTime cardOnDone, int trelloCampusId, params string[] members)
        //{
        //    // TODO: assign to properties
        //}

        public ReactionRuntime()
        {
            this.Report = ReportsLists.Runtime;
        }

        public override string ToString()
        {
            return this.Id + CSVExtension.SEPERATOR +
                this.TrelloCardId + CSVExtension.SEPERATOR +
                this.CardCreated + CSVExtension.SEPERATOR +
                this.CardOnProgress + CSVExtension.SEPERATOR + 
                this.CardIsDone + Environment.NewLine;
        }

        public string Headerline()
        {
            return String.Join(CSVExtension.SEPERATOR, "Id", "TrelloCardId", Model.CardCreated, Model.CardOnProgress, Model.CardIsDone, Environment.NewLine);
        }

        [NotMapped]
        public ReportsLists Report { get; set; }
    }
}
