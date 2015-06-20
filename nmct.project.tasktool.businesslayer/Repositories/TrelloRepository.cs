using nmct.project.tasktool.businesslayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrelloNet;
using nmct.project.tasktool.models;
using System.IO;
using System.Drawing;
using RestSharp;
using nmct.project.tasktool.models.Trello;
using nmct.project.tasktool.models.Archive;
using Thread = System.Threading.Tasks;

namespace nmct.project.tasktool.businesslayer.Repositories
{
    // -------------------------------------------------------------------------------------------------------------------------
    // Stijn Moreels - 27.05.2015
    //
    /* Comments:
     * Trello API KEY & Authorize -> in AppSettings from the Source Project 
     * 
     * Packages (Source Project):
     * Newtonsoft.Json                6.0.8 
     * RestSharp                      105.1.0
     * TrelloNet.Temp.Fork            0.6.2 
     * 
     */
    // -------------------------------------------------------------------------------------------------------------------------

    public class TrelloRepository : ITrelloRepository
    {
        public ITrello _trello;
        private const string _token = "token", _appkey = "appkey";

        // Constructor creation in Unity
        public TrelloRepository()
        {   
            _trello = new Trello(ConfigurationManager.AppSettings[_appkey]);
            _trello.Authorize(ConfigurationManager.AppSettings[_token]);
        }
        
        // ---------------------------------------- GET ----------------------------------------
        #region GET ITEMS
        public TrelloCard GetTrelloCard(ICardId cardId)
        {
            return _trello.Advanced.Get<TrelloCard>("/cards/" + cardId.GetCardId());
        }

        public string GetDescriptionFromCard(ICardId cardId)
        {
            // The Propertie "_value" is the propertie that Trello returns
            return _trello.Advanced.Get("/cards/" + cardId.GetCardId() + "/desc")._value;
        }

        public Member GetMemberForBoard(string username)
        {
            return _trello.Advanced.Get<Member>("/members/" + username);
        }

        public TrelloMember GetMemberForBoardOwnModel(string username)
        {
            return _trello.Advanced.Get<TrelloMember>("/members/" + username);
        }

        public Member GetMemberForBoardId(string id)
        {
            return _trello.Advanced.Get<Member>("/members/" + id);
        }

        public IEnumerable<TrelloLabel> GetAllLabelForBoard(IBoardId boardId)
        {
            return _trello.Advanced.Get<List<TrelloLabel>>("boards/" + boardId.GetBoardId() + "/labels");            
        }

        public IEnumerable<List> GetAllListsForBoard(IBoardId boardId)
        {
            return _trello.Lists.ForBoard(boardId, ListFilter.Open);
        }

        public IEnumerable<Member> GetAllMembersForBoard(IBoardId boardId, MemberFilter filter = MemberFilter.All)
        {
            return _trello.Members.ForBoard(boardId, filter);
        }

        public IEnumerable<Member> GetAllMembersForOrganisation(IOrganizationId organisationId)
        {
            return _trello.Members.ForOrganization(organisationId);
        }

        public IEnumerable<Card> GetAllCardsForBoard(IBoardId boardId, BoardCardFilter filter = BoardCardFilter.All)
        {
            return _trello.Cards.ForBoard(boardId, filter);
        }

        public IEnumerable<TrelloArchiveCard> GetAllArchiveCardsForBoard(IBoardId boardId)
        {
            return _trello.Advanced.Get<List<TrelloArchiveCard>>("/boards/" + boardId.GetBoardId() + "/cards?filter=closed");
        }

        public IEnumerable<TrelloCard> GetAllCardsForBoard(IBoardId boardId)
        {
            return _trello.Advanced.Get<List<TrelloCard>>("/boards/" + boardId.GetBoardId() + "/cards");
        }

        public IEnumerable<TrelloCard> GetAllLimitCardsForBoard(IBoardId boardId, int limit = 1000)
        {
            return _trello.Advanced.Get<List<TrelloCard>>("/boards/" + boardId.GetBoardId() + "/cards?limit=" + limit);
        }

        public async Task<IEnumerable<TrelloCard>> GetAllCardsForBoardAsync(IBoardId boardId)
        {
            return await _trello.Async.Advanced.Get<List<TrelloCard>>("/boards/" + boardId.GetBoardId() + "/cards");
        }

        public IEnumerable<Card> GetAllCardsForMember(IMemberId memberId)
        {
            return _trello.Cards.ForMember(memberId, CardFilter.Open);
        }

        public IEnumerable<TrelloCard> GetAllCardsForMemberOwnModel(IMemberId memberId)
        {
            return _trello.Advanced.Get<List<TrelloCard>>("members/" + memberId.GetMemberId() + "/cards");
        }

        public IEnumerable<TrelloCard> GetCardsForBoardForMember(IBoardId boardId, IMemberId memberId)
        {
            IEnumerable<TrelloCard> cards =  _trello.Advanced
                .Get<List<TrelloCard>>("boards/" + boardId.GetBoardId() + "/members/" + memberId.GetMemberId() + "/cards?fields=name,labels,badges");
            return cards;
        }

        public IEnumerable<TrelloCard> GetAllCardsForList(IListId listId, CardFilter filter = CardFilter.All)
        {
            return _trello.Advanced.Get<List<TrelloCard>>("lists/" + listId.GetListId() + "/cards");
        }

        public IEnumerable<TrelloAttachment> GetAllAttachmentsFromCard(ICardId cardId)
        {
            return _trello.Advanced.Get<List<TrelloAttachment>>("cards/" + cardId.GetCardId() + "/attachments");
        }

        public IEnumerable<TrelloComment> GetAllCommentsForCard(ICardId cardId, string filter = "commentCard")
        {
            return _trello.Advanced.Get<List<TrelloComment>>("/cards/" + cardId.GetCardId() + "/actions", arguments: new { filter = filter });
        }

        public IEnumerable<TrelloActionMoveCard> GetAllActionsForBoard(IBoardId boardId, string filter = "updateCard")
        {
            return _trello.Advanced.Get<List<TrelloActionMoveCard>>("/boards/" + boardId.GetBoardId() + "/actions?filter=" + filter);
        }

        public IEnumerable<TrelloActionMoveCard> GetAllMoveActionsForCard(ICardId cardId, string filter = "updateCard")
        {
            return _trello.Advanced.Get<List<TrelloActionMoveCard>>("/cards/" + cardId.GetCardId() + "/actions?filter=" + filter);
        }

        public IEnumerable<TrelloActionCreateCard> GetAllCreateActionsForCard(ICardId cardId, string filter = "createCard")
        {
            return _trello.Advanced.Get<List<TrelloActionCreateCard>>("/cards/" + cardId.GetCardId() + "/actions?filter=" + filter);
        }
        #endregion
        // -------------------------------------------------------------------------------------

        // ---------------------------------------- CREATE -------------------------------------
        #region CREATE ITEMS
        public void CreateVirtualMember(IBoardId boardId, string email, string fullname)
        {
            _trello.Boards.AddMember(boardId, email, fullname, BoardMemberType.Normal);
        }


        public void CreateLabelForBoard(IBoardId boardId, string name, string color = "null")
        {
            _trello.Advanced.Post("/labels/", arguments: new { name = name, color = color, idBoard = boardId.GetBoardId() });
        }

        public void AddAttachmentToCard(ICardId cardId, string path, string name, string fileName, string mimeType = "image/jpeg")
        {
            //_trello.Cards.AddAttachment(cardId, new FileAttachment(path, name));
            //_trello.Cards.AddAttachment(cardId, new BytesAttachment(File.ReadAllBytes(path), name, fileName));
            
            //using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            //{
            //    Bitmap img = new Bitmap(stream);
            //    _trello.Advanced.Post("/cards/" + cardId.GetCardId() + "/attachments",
            //            arguments: new { file = img, name = fileName, mimetype = System.Net.Mime.MediaTypeNames.Image.Jpeg }); 
                
            //}

            //var restClient = new RestClient("https://trello.com/1/cards/" + cardId.GetCardId() + "/attachments");
            //var restRequest = new RestRequest(Method.POST);
            //restRequest.AddFile(name, path);
            //var restResponse = restClient.Execute(restRequest);
        }
        #endregion
        // -------------------------------------------------------------------------------------

        // ---------------------------------------- UPDATE -------------------------------------
        #region UPDATE ITEMS
        public void ChangeDescriptionForCard(ICardId cardId, string description)
        {
            _trello.Cards.ChangeDescription(cardId, description);
        }

        public void MakeNewCommentToCard(ICardId cardId, string comment)
        {
            _trello.Advanced.Post("/cards/" + cardId.GetCardId() + "/actions/comments", arguments: new { text = comment });
        }

        // --------------------------------------
        // No support in the Trello.NET Library?
        // Solution -> .Advanced() -> Trello API
        // --------------------------------------
        public void MoveCardToDifferentList(ICardId cardId, IListId listId)
        {
            _trello.Advanced.Put("/cards/" + cardId.GetCardId() + "/idList", arguments: new { value = listId.GetListId() });
        }
        #endregion
        // -------------------------------------------------------------------------------------

        // ---------------------------------------- DELETE -------------------------------------
        // Used for the Settings Page -> Change Permissions for Members (ex. Worker -> Operator)
        //
        #region DELETE ITEMS
        public void DeleteMemberForBoard(IBoardId boardId, IMemberId memberId)
        {
            _trello.Boards.RemoveMember(boardId, memberId);
        }
        #endregion
        // -------------------------------------------------------------------------------------
    }
}
