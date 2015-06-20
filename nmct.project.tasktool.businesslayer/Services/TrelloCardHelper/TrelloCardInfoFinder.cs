using nmct.project.tasktool.businesslayer.Repositories.Interfaces;
using nmct.project.tasktool.businesslayer.Services.Interfaces;
using nmct.project.tasktool.businesslayer.Services.TrelloCardHelper.Interfaces;
using nmct.project.tasktool.models;
using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Extensions;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.businesslayer.Services.TrelloCardHelper
{
    // -------------------------------------------------------------------------------------------------------------------------
    // Stijn Moreels - 10.06.2015
    // 
    // Find members for membersId's
    // -------------------------------------------------------------------------------------------------------------------------
    public class TrelloCardInfoFinder : ITrelloCardInfoFinder
    {
        private ITrelloRepository _trelloRepository = null;
        private ITrelloMemberService _trelloMemberService = null;
        private IStaffService _staffService = null;

        public TrelloCardInfoFinder(ITrelloRepository trelloRepository, ITrelloMemberService trelloMemberService, IStaffService staffService)
        {
            this._trelloRepository = trelloRepository;
            this._trelloMemberService = trelloMemberService;
            this._staffService = staffService;
        }

        public TrelloList GetListInBoardByListId(string listId, string boardId)
        {
            string campusString = TrelloConstants.GetCampusNameForBoardId(boardId);
            CampusNames campus = TrelloConstants.GetCampusNameForString(campusString);
            CampusLists listEnum = TrelloConstants.CampusListsIds[campus][listId];
            string listName = StringEnum.GetStringValue(listEnum);

            return new TrelloList(Enum.GetName(typeof(CampusLists), listEnum), CampusListsExtension.AsDisplayString(listEnum), listId);
        }

        public IEnumerable<TrelloMember> FindAllMembersForCard(string[] listMembers, ref List<TrelloMember> refMembers)
        {
            List<TrelloMember> members = new List<TrelloMember>();
            for (int i = 0, l = listMembers.Length; i < l; i++)
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                TrelloMember member = this._trelloMemberService.GetMember(listMembers[i]);
                watch.Stop();

                StaffRole staffRole = this._staffService.GetUserForTrelloUsername(member.Name);
                if (staffRole != null)
                    member.FullName = staffRole.FullName;
                member.FullName = member.FullName ?? member.Name;

                if (members.Find(m => m.Id == member.Id) == null)
                    members.Add(member);
                
                if (refMembers.Find(m => m.Id == member.Id) == null)
                    refMembers.Add(member);
            }
            return members;
        }
    }
}
