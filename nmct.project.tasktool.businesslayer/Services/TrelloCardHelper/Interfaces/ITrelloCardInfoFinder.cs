using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmct.project.tasktool.businesslayer.Services.TrelloCardHelper.Interfaces
{
    public interface ITrelloCardInfoFinder
    {
        TrelloList GetListInBoardByListId(string listId, string boardId);
        IEnumerable<TrelloMember> FindAllMembersForCard(string[] listMembers, ref List<TrelloMember> refMembers);
    }
}
