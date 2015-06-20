using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models.Trello
{
    public class TrelloCampus
    {
        public int Id { get; set; }
        public String TrelloIdCampus { get; set; }

        public String CampusName { get; set; }
    }
}
