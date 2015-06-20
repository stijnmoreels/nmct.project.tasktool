using Newtonsoft.Json;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrelloNet;

namespace nmct.project.tasktool.models
{
    public class Task
    {
        public string CardId { get; set; }
        public string BoardId { get; set; }
        public TrelloList List { get; set; }
        public Campus Campus { get; set; }
        public Urgency Urgency { get; set; }
        public List<TrelloMember> Members { get; set; }
        public Room Room { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Magnitude Magnitude { get; set; }
        public string CreationDate { get; set; }
        public int AttachmentsCount { get; set; }
        public int CommentsCount { get; set; }
        public int MemberCount { get; set; }

        public string[] MemberIds { get; set; }

        public string DueDate { get; set; }
    }
}
