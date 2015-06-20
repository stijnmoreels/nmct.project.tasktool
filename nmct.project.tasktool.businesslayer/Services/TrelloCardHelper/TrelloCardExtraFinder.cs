using nmct.project.tasktool.businesslayer.Repositories.Interfaces;
using nmct.project.tasktool.businesslayer.Services.Interfaces;
using nmct.project.tasktool.businesslayer.Services.TrelloCardHelper.Interfaces;
using nmct.project.tasktool.models;
using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Extensions;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrelloNet;

namespace nmct.project.tasktool.businesslayer.Services.TrelloCardHelper
{
    // -------------------------------------------------------------------------------------------------------------------------
    // Stijn Moreels - 10.06.2015
    // 
    // Features:
    // - Find Attachments
    // - Find Comments
    // - Find Members
    // - Basic Task Info
    // - ListName for ListId and BoardId
    // -------------------------------------------------------------------------------------------------------------------------
    public class TrelloCardExtraFinder : ITrelloCardExtraFinder
    {
        private ITrelloRepository _trelloRepository = null;
        private ITrelloCardTaskCreator _trelloCardTaskCreator = null;

        public TrelloCardExtraFinder(ITrelloCardTaskCreator trelloCardTaskCreator, ITrelloRepository trelloRepository)
        {
            this._trelloRepository = trelloRepository;
            this._trelloCardTaskCreator = trelloCardTaskCreator;
        }

        public IEnumerable<TrelloAttachment> FindAllAttachmentsForCard(string cardId)
        {
            IEnumerable<TrelloAttachment> attachments =
                this._trelloRepository.GetAllAttachmentsFromCard(new CardId(cardId));
            return attachments.Count() > 0 ? attachments : new List<TrelloAttachment>();
        }

        public IEnumerable<TrelloComment> FindAllCommentsForCard(string cardId)
        {
            IEnumerable<TrelloComment> comments =
                this._trelloRepository.GetAllCommentsForCard(new CardId(cardId));
            return comments.Count() > 0 ? comments : new List<TrelloComment>();
        }

        public List<TrelloMember> FindAllMembersForCard(string[] listMembers)
        {
            List<TrelloMember> members = new List<TrelloMember>();
            for (int i = 0, l = listMembers.Length; i < l; i++)
            {
                Member member = this._trelloRepository.GetMemberForBoardId(listMembers[i]);
                if (members.Find(m => m.Id == member.Id) == null)
                    members.Add(new TrelloMember(member));
            }
            return members;
        }

        public Task GetTaskInfo(string cardId)
        {
            TrelloCard card = this._trelloRepository.GetTrelloCard(new CardId(cardId));
            List<TrelloMember> refMembers = new List<TrelloMember>();
            List<Campus> campuses = new List<Campus>();
            List<Room> rooms = new List<Room>();
            List<Urgency> statuses = new List<Urgency>();
            List<Magnitude> magnitudes = new List<Magnitude>();
            return this._trelloCardTaskCreator.ConvertTrelloCardToTask(ref refMembers, campuses, rooms, statuses, magnitudes, card);
        }

        public TrelloList GetListInBoardByListId(string listId, string boardId)
        {
            string campusString = TrelloConstants.GetCampusNameForBoardId(boardId);
            CampusNames campus = TrelloConstants.GetCampusNameForString(campusString);
            CampusLists listEnum = TrelloConstants.CampusListsIds[campus][listId];
            string listName = StringEnum.GetStringValue(listEnum);

            return new TrelloList(Enum.GetName(typeof(CampusLists), listEnum), CampusListsExtension.AsDisplayString(listEnum), listId);
        }
    }
}
