using System;
namespace nmct.project.tasktool.models.Interfaces
{
    public interface IReactionRuntime
    {
        DateTime CardCreated { get; set; }
        DateTime? CardIsDone { get; set; }
        DateTime CardOnProgress { get; set; }
        int Id { get; set; }
        string[] MemberIds { get; set; }
        nmct.project.tasktool.models.Trello.TrelloCampus TrelloCampus { get; set; }
        int TrelloCampusId { get; set; }
        string TrelloCardId { get; set; }
        System.Collections.Generic.List<nmct.project.tasktool.models.Trello.TrelloMember> TrelloMembers { get; set; }
    }
}
