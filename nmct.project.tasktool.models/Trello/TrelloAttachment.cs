using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models.Trello
{
    public class TrelloAttachment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        
        private string _type;
        public string Type
        {
            get 
            {
                return this.Url.Split('.').LastOrDefault<string>();
            }
            set { _type = value; }
        }
        
    }
}
