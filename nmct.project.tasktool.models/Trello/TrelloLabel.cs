using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrelloNet;

namespace nmct.project.tasktool.models.Trello
{
    public class TrelloLabel : Card.Label
    {
        public string Id { get; set; }
        public string IdBoard { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public int Uses { get; set; }
    }
}
