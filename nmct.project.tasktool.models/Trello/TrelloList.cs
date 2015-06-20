using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models.Trello
{
    public class TrelloList
    {
        public string Id { get; set; }
        public string TrelloId { get; set; }
        public string Name { get; set; }

        public TrelloList()
        {

        }

        public TrelloList(string name)
        {
            this.Name = name;
        }

        public TrelloList(string id, string name, string trelloId)
        {
            this.Id = id;
            this.Name = name;
            this.TrelloId = trelloId;
        }
    }
}
