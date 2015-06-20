using nmct.project.tasktool.models.Trello;
using System;
namespace nmct.project.tasktool.models.Interfaces
{
    public interface ITrelloCard
    {
        System.Collections.Generic.List<TrelloNet.Card.Attachment> Attachments { get; set; }
        TrelloNet.Card.CardBadges Badges { get; set; }
        string Desc { get; set; }
        DateTime? Due { get; set; }
        string Id { get; set; }
        string IdBoard { get; set; }
        string IdList { get; set; }
        string[] IdMembers { get; set; }
        System.Collections.Generic.List<TrelloLabel> Labels { get; set; }
        string Name { get; set; }
        string ShortLink { get; set; }
    }
}
