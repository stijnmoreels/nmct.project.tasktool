using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
namespace nmct.project.tasktool.businesslayer.Services.TrelloCardHelper.Interfaces
{
    public interface ITrelloCardExtraFinder
    {
        System.Collections.Generic.IEnumerable<nmct.project.tasktool.models.Trello.TrelloAttachment> FindAllAttachmentsForCard(string cardId);
        System.Collections.Generic.IEnumerable<nmct.project.tasktool.models.Trello.TrelloComment> FindAllCommentsForCard(string cardId);
        System.Collections.Generic.List<nmct.project.tasktool.models.Trello.TrelloMember> FindAllMembersForCard(string[] listMembers);
        nmct.project.tasktool.models.Trello.TrelloList GetListInBoardByListId(string listId, string boardId);
        nmct.project.tasktool.models.Task GetTaskInfo(string cardId);
    }
}
