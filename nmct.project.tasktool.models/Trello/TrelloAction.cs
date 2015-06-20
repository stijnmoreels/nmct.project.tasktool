using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace nmct.project.tasktool.models.Trello
{
    public class TrelloActionMoveCard
    {
        public string Id { get; set; }
        public TrelloActionData Data { get; set; }
        public string Type { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }

    public class TrelloActionData
    {
        public TrelloActionDataList ListAfter { get; set; }
        public TrelloActionDataList ListBefore { get; set; }
        public TrelloActionDataList Board { get; set; }
        public TrelloActionDataList Card { get; set; }
    }

    public class TrelloActionDataList
    {
        public string Id { get; set; }
        [System.ComponentModel.DefaultValue("None")]
        public string Name { get; set; }
    }
}
