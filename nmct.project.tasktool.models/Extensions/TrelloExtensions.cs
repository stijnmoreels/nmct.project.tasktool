using nmct.project.tasktool.models.Archive;
using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models.Extensions
{
    public static class CampusListsExtension
    {
        public static string AsDisplayString(this CampusLists list)
        {
            switch (list)
            {
                case CampusLists.ToDo: return Properties.Trello.CampusLists.ToDo;
                case CampusLists.OnHold: return Properties.Trello.CampusLists.OnHold;
                case CampusLists.InProgress: return Properties.Trello.CampusLists.InProgress;
                case CampusLists.Done: return Properties.Trello.CampusLists.Done;
                case CampusLists.Blocked: return Properties.Trello.CampusLists.Blocked;

                default: throw new ArgumentOutOfRangeException("No DisplayName Found!");
            }
        }
    }   

    public static class UrgencyExtension
    {
        public static string AsDisplayString(this string urgency)
        {
            switch (urgency)
            {
                case "NotUrgent": return Properties.Urgency.Model.NotUrgent;
                case "LessUrgent": return Properties.Urgency.Model.LessUrgent;
                case "MoreUrgent": return Properties.Urgency.Model.MoreUrgent;
                case "VeryUrgent": return Properties.Urgency.Model.VeryUrgent;
                default: throw new ArgumentOutOfRangeException("No DisplayName Found!");
            }
        }
    }

    public static class TrelloArchiveCardExtension
    {
        public static IList<TrelloArchiveCard> AddRange(this IList<TrelloArchiveCard> destination, 
            IList<TrelloCard> source)
        {
            foreach(TrelloCard card in source)
            {
                destination.Add(new TrelloArchiveCard(card));
            }
            return destination;
        }
    }
}
