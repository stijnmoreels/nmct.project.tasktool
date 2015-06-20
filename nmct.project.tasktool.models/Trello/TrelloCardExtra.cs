using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models.Trello
{
    public class TrelloCardExtra
    {
        public List<TrelloAttachment> Attachments { get; set; }
        public List<TrelloComment> Comments { get; set; }
    }
}
