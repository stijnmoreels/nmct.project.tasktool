using nmct.project.tasktool.models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmct.project.tasktool.models
{
    public class Urgency : ILabel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TrelloId { get; set; }

        public Urgency()
        {

        }

        public Urgency(string name)
        {
            this.Name = name;
        }

        public Urgency(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public Urgency(string id, string name, string trelloId)
        {
            this.Id = id;
            this.Name = name;
            this.TrelloId = trelloId;
        }
    }
}
