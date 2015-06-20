using nmct.project.tasktool.models;
using nmct.project.tasktool.models.Archive;
using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using TrelloNet;
using Thread = System.Threading.Tasks;

namespace nmct.project.tasktool.businesslayer.Services.Interfaces
{
    public interface ITrelloService
    {
        // -------------------- GET --------------------
        Member GetMemberForUsername(string username);
        List<Task> GetOverviewContent(ref List<TrelloMember> refMembers, ref List<Campus> campuses, ref List<Room> rooms, ref List<Urgency> statuses, ref List<Magnitude> magnitudes, bool allCampusses = true, CampusNames filterCampus = CampusNames.GKG);
        Thread.Task<List<Task>> GetOverviewContentAsync(List<TrelloMember> members,
            List<Campus> campuses, List<Room> rooms, List<Urgency> statuses,
            List<Magnitude> magnitudes, bool allCampusses = true, CampusNames filterCampus = CampusNames.GKG);
        List<Task> GetPersonalOverviewContent(string username, ref List<Campus> campuses, ref List<Room> rooms, ref List<Urgency> statuses, ref List<Magnitude> magnitudes, ref List<TrelloMember> refMembers);
        IEnumerable<TrelloCard> GetAllTasksForList(IBoardId boardId, string listName = "Done");
        IEnumerable<TrelloCard> GetAllTasksForList(CampusNames campus, string listName = "Done");
        Thread.Task<List<TrelloCard>> GetAllCardsFromCampusesAsync(bool all = true, CampusNames filterCampus = CampusNames.GKG);
        List<TrelloCard> GetAllCardsFromCampuses(bool all = true, CampusNames filterCampus = CampusNames.GKG);
        IEnumerable<TrelloArchiveCard> GetAllTasksForArchive(CampusNames campus);
        IEnumerable<Card> GetAllTasksForMember(TrelloNet.IMemberId memberId);
        IEnumerable<Member> GetAllMembersForOrganisation(IOrganizationId organisationId);
        IEnumerable<Member> GetAllMembersForBoard(CampusNames campus);
        IEnumerable<TrelloCard> GetFilterCardsFor(IBoardId boardId, IMemberId memberId = null, params string[] labelNames);
        IEnumerable<TrelloComment> GetAllActionsForCard(string cardId, string filter = "commentCard");
        IEnumerable<TrelloActionMoveCard> GetAllActionForUpdateCard(string cardId);
        IEnumerable<TrelloArchiveCard> GetTasksFromBoardAndArchiveWithReactionRuntimesForCampus(string campus, int limit = 1000);
        IEnumerable<Campus> GetAllCampusses();

        // -------------------- CREATE --------------------
        Boolean AddExternalUser(String name);
        void CreateMemberFor(CampusNames campus, IEnumerable<string> users, string fakeEmail = "@worker.be");
        void CreateMemberFor(CampusNames campus, string email, string user);
        void CreateLabel(CampusNames campus, string name, string color = null);
        void CreateLabel(CampusNames campus, Urgencies name, string color = null);
        void MakeNewCommentToCard(string cardId, string comment);

        // -------------------- UPDATE --------------------
        void UpdateMembers(List<StaffRole> staff);
        void MoveCardToDifferentList(string cardId, string boardId, string listName = "Done");

        // -------------------- DELETE --------------------
        void DeleteMemberFor(CampusNames campus, string username);
        Boolean DeleteMember(string username);
    }
}
