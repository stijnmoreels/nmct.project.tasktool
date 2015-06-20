using nmct.project.tasktool.models;
using nmct.project.tasktool.models.Archive;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using TrelloNet;
using Thread = System.Threading.Tasks;

namespace nmct.project.tasktool.businesslayer.Repositories.Interfaces
{
    public interface ITrelloRepository
    {
        // -------------------- GET --------------------
        TrelloCard GetTrelloCard(ICardId cardId);
        Member GetMemberForBoard(string username);
        string GetDescriptionFromCard(ICardId cardId);
        TrelloMember GetMemberForBoardOwnModel(string username);
        Member GetMemberForBoardId(string id);
        IEnumerable<TrelloLabel> GetAllLabelForBoard(IBoardId boardId);
        IEnumerable<List> GetAllListsForBoard(IBoardId boardId);
        IEnumerable<Card> GetAllCardsForMember(IMemberId memberId);
        IEnumerable<TrelloCard> GetAllCardsForMemberOwnModel(IMemberId memberId);
        IEnumerable<Member> GetAllMembersForBoard(IBoardId boardId, MemberFilter filter = MemberFilter.All);
        IEnumerable<Member> GetAllMembersForOrganisation(IOrganizationId organisationId);
        IEnumerable<TrelloCard> GetAllCardsForBoard(IBoardId boardId);
        IEnumerable<Card> GetAllCardsForBoard(IBoardId boardId, BoardCardFilter filter = BoardCardFilter.All);
        IEnumerable<TrelloCard> GetAllLimitCardsForBoard(IBoardId boardId, int limit = 1000);
        Thread.Task<IEnumerable<TrelloCard>> GetAllCardsForBoardAsync(IBoardId boardId);
        IEnumerable<TrelloArchiveCard> GetAllArchiveCardsForBoard(IBoardId boardId);
        IEnumerable<TrelloCard> GetAllCardsForList(IListId listId, CardFilter filter = CardFilter.All);
        IEnumerable<TrelloCard> GetCardsForBoardForMember(IBoardId boardId, IMemberId memberId);
        IEnumerable<TrelloAttachment> GetAllAttachmentsFromCard(ICardId cardId);
        IEnumerable<TrelloComment> GetAllCommentsForCard(ICardId cardId, string filter = "commentCard");
        IEnumerable<TrelloActionMoveCard> GetAllActionsForBoard(IBoardId boardId, string filter = "updateCard");
        IEnumerable<TrelloActionMoveCard> GetAllMoveActionsForCard(ICardId cardId, string filter = "updateCard");
        IEnumerable<TrelloActionCreateCard> GetAllCreateActionsForCard(ICardId cardId, string filter = "createCard");

        // -------------------- CREATE --------------------
        void CreateVirtualMember(IBoardId boardId, string email, string fullname);
        void CreateLabelForBoard(IBoardId boardId, string name, string color = "null");
        void AddAttachmentToCard(ICardId cardId, string path, string name, string fileName, string mimeType = "image/jpeg");
   

        // -------------------- UPDATE --------------------
        void ChangeDescriptionForCard(ICardId cardId, string description);
        void MoveCardToDifferentList(ICardId cardId, IListId listId);
        void MakeNewCommentToCard(ICardId cardId, string comment);

        // -------------------- DELETE --------------------
        void DeleteMemberForBoard(IBoardId boardId, IMemberId memberId);
    }
}