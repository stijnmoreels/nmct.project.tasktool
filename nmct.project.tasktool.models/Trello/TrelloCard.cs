using Newtonsoft.Json;
using nmct.project.tasktool.models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrelloNet;

namespace nmct.project.tasktool.models.Trello
{
    public class TrelloCard : ITrelloCard
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string IdBoard { get; set; }
        public string IdList { get; set; }
        public string ShortLink { get; set; }
        public string[] IdMembers { get; set; }

        public DateTime? Due { get; set; } 
        public List<TrelloLabel> Labels { get; set; }
        public List<Card.Attachment> Attachments { get; set; }
        public Card.CardBadges Badges { get; set; }

        public TrelloLabel GetLabelFromThisLabels(string name)
        {
            if(this.Labels != null && !name.Equals(String.Empty))
               return this.Labels
                   .Where(l => l.Name.Equals(name))
                   .SingleOrDefault<TrelloLabel>();
            return null;
        }
    }
}
