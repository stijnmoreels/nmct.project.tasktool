using Newtonsoft.Json;
using nmct.project.tasktool.models.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models.Trello
{
    public class TrelloMember
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        
        //[JsonIgnore]
        [NotMapped]
        public string FullName { get; set; }

        [JsonIgnore]
        public List<ReactionRuntime> Reactions { get; set; }

        public TrelloMember()
        {

        }

        public TrelloMember(string name)
        {
            this.Name = name;
        }

        public TrelloMember(TrelloNet.Member member)
        {
            this.Id = member.Id;
            this.Name = member.FullName;
        }
    }
}
