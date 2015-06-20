using nmct.project.tasktool.businesslayer.Repositories.Interfaces;
using nmct.project.tasktool.businesslayer.Services.Interfaces;
using nmct.project.tasktool.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrelloNet;
using System.Data.Entity;
using nmct.project.tasktool.models.Interfaces;
using System.Diagnostics;
using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Trello;
using System.Text.RegularExpressions;
using System.Globalization;
using nmct.project.tasktool.models.Extensions;
using nmct.project.tasktool.models.Archive;
using nmct.project.tasktool.models.Reports;
using nmct.project.tasktool.businesslayer.Services.TrelloCardHelper;
using nmct.project.tasktool.businesslayer.Services.TrelloCardHelper.Interfaces;
using Thread = System.Threading.Tasks;
using nmct.project.tasktool.businesslayer.Async;

namespace nmct.project.tasktool.businesslayer.Services
{
    // -------------------------------------------------------------------------------------------------------------------------
    // Stijn Moreels - 27.05.2015
    // 
    // Dependencies:
    // - ITrelloRepository
    // - TrelloConstants
    // - TrelloCard
    // - TrelloLabel
    // - ITrelloCardExtraFinder
    // - ITrelloCardTaskCreator
    // - ITrelloCardInfoFinder
    // -------------------------------------------------------------------------------------------------------------------------

    public class TrelloService : ITrelloService
    {
        private ITrelloRepository _trelloRepository = null;
        private ITrelloMemberService _trelloMemberService = null;
        private IStaffService _staffService = null;
        private IReportService _reportService = null;

        private ITrelloCardExtraFinder _trelloCardExtraFinder = null;
        private ITrelloCardTaskCreator _trelloCardTaskCreator = null;
        private ITrelloCardInfoFinder _trelloCardInfoFinder = null;

        public TrelloService(ITrelloRepository trelloRepository, ITrelloMemberService trelloMemberService, 
            IStaffService staffService, IReportService reportService, ITrelloCardExtraFinder trelloCardExtraFinder,
            ITrelloCardTaskCreator trelloCardTaskCreator, ITrelloCardInfoFinder  trelloCardInfoFinder)
        {
            this._trelloRepository = trelloRepository;
            this._trelloMemberService = trelloMemberService;
            this._staffService = staffService;
            this._reportService = reportService;
            this._trelloCardExtraFinder = trelloCardExtraFinder;
            this._trelloCardTaskCreator = trelloCardTaskCreator;
            this._trelloCardInfoFinder = trelloCardInfoFinder;
        }

        // -------------------------------------------------------------------------
        // Used for the Overview in the Web Application (All & Personal)
        // -------------------------------------------------------------------------
        public IEnumerable<Card> GetAllTasksForMember(IMemberId memberId)
        {
            return _trelloRepository.GetAllCardsForMember(memberId);
        }

        public IEnumerable<Member> GetAllMembersForOrganisation(IOrganizationId organisationId)
        {
            return _trelloRepository.GetAllMembersForOrganisation(organisationId);
        }

        public IEnumerable<Member> GetAllMembersForBoard(CampusNames campus)
        {
            IBoardId boardId = GetBoardIdFromConstants(campus);
            return _trelloRepository.GetAllMembersForBoard(boardId);
        }

        public IEnumerable<TrelloCard> GetAllTasksForList(IBoardId boardId, string listName = "Done")
        {
            List list = GetListFromBoardByListName(boardId, listName);
            return this._trelloRepository.GetAllCardsForList(list);
        }

        public IEnumerable<TrelloCard> GetAllTasksForList(CampusNames campus, string listName = "Done")
        {
            List list = GetListFromBoardByListName(new BoardId(StringEnum.GetStringValue(campus)), listName);
            if (list != null)
                return this._trelloRepository.GetAllCardsForList(list);
            else
                return new List<TrelloCard>();
        }

        public IEnumerable<TrelloArchiveCard> GetAllTasksForArchive(CampusNames campus)
        {
            BoardId boardId = new BoardId(StringEnum.GetStringValue(campus));
            return this._trelloRepository.GetAllArchiveCardsForBoard(boardId);
        }

        private List GetListFromBoardByListName(IBoardId boardId, string listName)
        {
            List list = _trelloRepository.GetAllListsForBoard(boardId).ToList()
                .Where(l => l.Name.Equals(listName))
                .SingleOrDefault<List>();
            return list;
        }

        public IEnumerable<TrelloComment> GetAllActionsForCard(string cardId, string filter = "commentCard")
        {
            return this._trelloRepository.GetAllCommentsForCard(new CardId(cardId), filter);
        }

        public IEnumerable<TrelloActionMoveCard> GetAllActionForUpdateCard(string cardId)
        {
            return this._trelloRepository.GetAllMoveActionsForCard(new CardId(cardId), "updateCard:idList");
        }

        #region ARCHIVE PROCESS
        // ---------------------------------------------------------------------------------------------------------------------------------------
        public IEnumerable<TrelloArchiveCard> GetTasksFromBoardAndArchiveWithReactionRuntimesForCampus(string campus, int limit = 1000)
        {
            CampusNames campusEnum = TrelloConstants.GetCampusNameForString(campus);
            IEnumerable<TrelloArchiveCard> tasksArchive = GetAllTasksForArchive(campusEnum);
            //IEnumerable<TrelloCard> tasksDone = GetAllTasksForList(campusEnum);
            IEnumerable<TrelloCard> tasksAll = this._trelloRepository.GetAllLimitCardsForBoard(TrelloConstants.CAMPUSES[campusEnum], limit);

            tasksArchive = GetAllReactionRuntimesForEveryTask(campusEnum, tasksArchive, tasksAll);
            if (tasksArchive != null)
            {
                List<TrelloArchiveCard> archive = tasksArchive.ToList<TrelloArchiveCard>();
               archive
                    .ForEach(t => t.CardCreated = TrelloCardIdConverter.GetDatetimeFromCardId(t.Id).ToString(CultureConstants.DATETIME_FORMAT));
               archive
                   .ForEach(t => t.List = this._trelloCardExtraFinder.GetListInBoardByListId(t.IdList, t.IdBoard));
            }

            return tasksArchive;
        }

        private IEnumerable<TrelloArchiveCard> GetAllReactionRuntimesForEveryTask(CampusNames campusEnum, IEnumerable<TrelloArchiveCard> tasks, IEnumerable<TrelloCard> tasksDone)
        {
            List<TrelloArchiveCard> taskList = tasks.ToList<TrelloArchiveCard>();
            taskList.ForEach(t => t.InArchive = true);
            tasks = TrelloArchiveCardExtension.AddRange(taskList, tasksDone.ToList<TrelloCard>());
            List<ReactionRuntime> runtimes = this._reportService.GetAllReactionRuntimesForCampus(campusEnum);

            foreach (TrelloArchiveCard task in tasks)
            {
                FindReactionRuntime(runtimes, task);
                FindLabels(task);
                //FindMembers(task);
            }
            return tasks;
        }

        private void FindReactionRuntime(List<ReactionRuntime> runtimes, TrelloArchiveCard task)
        {
            ReactionRuntime runtime = runtimes.Find(r => r.TrelloCardId.Equals(task.Id));
            if (runtime != null)
                task.InsertReactionRuntime(runtime);
        }

        private void FindLabels(TrelloArchiveCard task)
        {
            if (task.Labels != null)
                for (int i = 0, l = task.Labels.Count; i < l; i++)
                {
                    Room room = new Room();
                    Urgency urgency = new Urgency();
                    Magnitude magnitude = new Magnitude();
                    this._trelloCardTaskCreator.FindInfoInLabels(task.Labels, ref room, ref urgency, ref magnitude, i);

                    task.Room = room.Id == null ? null : room;
                    task.Urgency = urgency.Id == null ? null : urgency;
                    task.Magnitude = magnitude.Id == null ? null : magnitude;
                }
        }

        private void FindMembers(TrelloArchiveCard task)
        {
            if (task.IdMembers != null)
                task.TrelloMembers = this._trelloCardExtraFinder.FindAllMembersForCard(task.IdMembers);
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------
        #endregion

        // -------------------------------------------------------------------------
        // Update Items
        // -------------------------------------------------------------------------
        public void MoveCardToDifferentList(string cardId, string boardId, string listName = "Done")
        {
            List listId = GetListFromBoardByListName(new BoardId(boardId), listName);
            this._trelloRepository.MoveCardToDifferentList(new CardId(cardId), listId);
        }

        // -------------------------------------------------------------------------
        // Create Items
        // -------------------------------------------------------------------------
        public void MakeNewCommentToCard(string cardId, string comment)
        {
            this._trelloRepository.MakeNewCommentToCard(new CardId(cardId), comment);
        }

        // -------------------------------------------------------------------------
        // The filter method has the following functionalities
        // - Filter Board
        // - Filter Member
        // - Filter Multiple Labels
        // -> Board is Required !!!
        // -------------------------------------------------------------------------
        #region FILTER FUNCTIONALITIES
        public IEnumerable<TrelloCard> GetFilterCardsFor(IBoardId boardId, IMemberId memberId = null, params string[] labelNames)
        {
            IEnumerable<TrelloCard> filterCards = new List<TrelloCard>();
            filterCards = FilterBoardWithMember(boardId, memberId, filterCards);
            return FilterBoardWithLabel(labelNames, filterCards);
        }

        private IEnumerable<TrelloCard> FilterBoardWithLabel(string[] labelNames, IEnumerable<TrelloCard> filterCards)
        {
            if (labelNames.Length > 0)
            {
                IList<TrelloCard> cardsForBoardForMemberForLabel = new List<TrelloCard>();
                for (int i = 0, l = labelNames.Length; i < l; i++)
                    foreach (TrelloCard card in filterCards)
                        if (card.GetLabelFromThisLabels(labelNames[i]) != null
                            && !cardsForBoardForMemberForLabel.Contains(card))
                            cardsForBoardForMemberForLabel.Add(card);
                return cardsForBoardForMemberForLabel;
            }
            return filterCards;
        }

        private IEnumerable<TrelloCard> FilterBoardWithMember(IBoardId boardId,
            IMemberId memberId, IEnumerable<TrelloCard> filterCards)
        {
            if (memberId != null)
                filterCards = _trelloRepository.GetCardsForBoardForMember(boardId, memberId);
            else
                filterCards = _trelloRepository.GetAllCardsForBoard(boardId);
            return filterCards;
        }
        #endregion

        // -------------------------------------------------------------------------
        // Create Labels
        // -------------------------------------------------------------------------
        public void CreateLabel(CampusNames campus, string name, string color = null)
        {
            IBoardId boardId = GetBoardIdFromConstants(campus);
            _trelloRepository.CreateLabelForBoard(boardId, name, color);
        }

        public void CreateLabel(CampusNames campus, Urgencies name, string color = null)
        {
            IBoardId boardId = GetBoardIdFromConstants(campus);
            _trelloRepository.CreateLabelForBoard(boardId, name.ToString(), color);
        }

        // -------------------------------------------------------------------------
        // Used for the TASKBOT & the Settings feature in de Web Application
        // -------------------------------------------------------------------------
        #region PROCESS MEMBER
        public void CreateMemberFor(CampusNames campus, string email, string user)
        {
            IBoardId boardId = GetBoardIdFromConstants(campus);
            _trelloRepository.CreateVirtualMember(boardId, email, user);
        }

        public void CreateMemberFor(CampusNames campus, IEnumerable<string> users, string fakeEmail = "@worker.be")
        {
            IBoardId boardId = GetBoardIdFromConstants(campus);
            foreach (string user in users)
                _trelloRepository.CreateVirtualMember(boardId, String.Concat(user, fakeEmail), user);
        }

        private IBoardId GetBoardIdFromConstants(CampusNames campus)
        {
            return TrelloConstants.CAMPUSES[campus];
        }

        public void DeleteMemberFor(CampusNames campus, string username)
        {
            Member member = _trelloRepository.GetMemberForBoard(username);
            IBoardId boardId = GetBoardIdFromConstants(campus);
            _trelloRepository.DeleteMemberForBoard(boardId, member);
        }
         
        public bool DeleteMember(string username)
        {
            foreach (CampusNames camp in Enum.GetValues(typeof(CampusNames)))
            {
                try
                {
                    DeleteMemberFor(camp, username);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public Member GetMemberForUsername(string username)
        {
            return _trelloRepository.GetMemberForBoard(username);
        }

        public void UpdateMembers(List<StaffRole> staff)
        {
            foreach (StaffRole stf in staff)
            {
                if (stf.Changed)
                {
                    foreach (CampusNames camp in Enum.GetValues(typeof(CampusNames)))
                    {
                        try
                        {
                            DeleteMemberFor(camp, stf.TrelloUsername);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                        if (stf.RoleId == TrelloConstants.WORKERID)
                            CreateMemberFor(camp, stf.NameStaff + "@worker.be", stf.NameStaff);
                        else if (stf.RoleId == TrelloConstants.OPERATORID || stf.RoleId == TrelloConstants.ADMINID)
                            CreateMemberFor(camp, stf.NameStaff +"@" + TrelloConstants.EMAILDOMAIN, stf.NameStaff);
                    }
                }
            }
        }

        public Boolean AddExternalUser(String name)
        {
            foreach (CampusNames camp in Enum.GetValues(typeof(CampusNames)))
            {
                try
                {
                    CreateMemberFor(camp, name + "@external.worker.be", name);

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        // -------------------------------------------------------------------------
        // Used for the Overview feature in de Web Application
        // -------------------------------------------------------------------------
        #region PROCESS OVERVIEW CONTENT
        public List<Task> GetPersonalOverviewContent(string username, ref List<Campus> campuses, ref List<Room> rooms,
            ref List<Urgency> statuses, ref List<Magnitude> magnitudes, ref List<TrelloMember> refMembers)
        {
            OverviewContent overview = new OverviewContent();
            List<Task> tasks = new List<Task>();
            TrelloMember member = this._trelloRepository.GetMemberForBoardOwnModel(username);
            IEnumerable<TrelloCard> cards = this._trelloRepository.GetAllCardsForMemberOwnModel(new MemberId(member.Id));
            if (cards.Count() > 0)
            {
                tasks = this._trelloCardTaskCreator.ConvertTrelloCardsToTasks(refMembers, campuses, rooms, statuses, magnitudes, cards.ToList<TrelloCard>());
                //tasks = ConvertTrelloCardsToTasks(ref refMembers, campuses, rooms, statuses, magnitudes, cards.ToList<TrelloCard>());
                refMembers = new List<TrelloMember>();
                this._trelloCardInfoFinder.FindAllMembersForCard(new String[]{member.Id}, ref refMembers).ToList<TrelloMember>();
            }
            return tasks;
        }

        public List<Task> GetOverviewContent(ref List<TrelloMember> refMembers,
            ref List<Campus> campuses, ref List<Room> rooms, ref List<Urgency> statuses,
            ref List<Magnitude> magnitudes, bool allCampusses = true, CampusNames filterCampus = CampusNames.GKG)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<TrelloCard> cards = GetAllCardsFromCampuses(allCampusses, filterCampus);
            //List<TrelloCard> cards = AsyncHelper.RunSync<List<TrelloCard>>(() => GetAllCardsFromCampusesAsync(allCampusses, filterCampus));
            watch.Stop();
            List<Task> tasks = this._trelloCardTaskCreator.ConvertTrelloCardsToTasks(refMembers, campuses, rooms, statuses, magnitudes, cards);
            return tasks;
        }

        public async Thread.Task<List<Task>> GetOverviewContentAsync(List<TrelloMember> members,
            List<Campus> campuses, List<Room> rooms, List<Urgency> statuses,
            List<Magnitude> magnitudes, bool allCampusses = true, CampusNames filterCampus = CampusNames.GKG)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //List<TrelloCard> cards = GetAllCardsFromCampuses(allCampusses, filterCampus);
            List<TrelloCard> cards = await GetAllCardsFromCampusesAsync(allCampusses, filterCampus);
            watch.Stop();

            watch.Restart();
            List<Task> tasks = this._trelloCardTaskCreator.ConvertTrelloCardsToTasks(members, campuses, rooms, statuses, magnitudes, cards);
            watch.Start();
            return tasks;
        }

        public async Thread.Task<List<TrelloCard>> GetAllCardsFromCampusesAsync(bool all = true, CampusNames filterCampus = CampusNames.GKG)
        {
            List<TrelloCard> cards = new List<TrelloCard>();
            List<TrelloCard> filterCards = new List<TrelloCard>();
            if (all)
            {
                IEnumerable<TrelloCard> allCards = await this._trelloRepository.GetAllCardsForBoardAsync(TrelloConstants.CAMPUSES[filterCampus]);
                cards.AddRange(allCards);
                return cards;
            }
            else
            {
                List<TrelloCard> allCards = this._trelloRepository.GetAllCardsForBoard(TrelloConstants.CAMPUSES[filterCampus]).ToList<TrelloCard>();
                allCards = allCards.Where(c => c.IdMembers.Length > 0).ToList<TrelloCard>();
                filterCards = new List<TrelloCard>();
                allCards.ForEach(c => TrelloConstants.IsListNameEqualsTo(c.IdBoard, c.IdList, filterCards, c));
            }
            return filterCards;
        }

        public List<TrelloCard> GetAllCardsFromCampuses(bool all = true, CampusNames filterCampus = CampusNames.GKG)
        {
            List<TrelloCard> cards = new List<TrelloCard>();
            List<TrelloCard> filterCards = new List<TrelloCard>();
            if (all)
            {
                IEnumerable<TrelloCard> allCards =  this._trelloRepository.GetAllCardsForBoard(TrelloConstants.CAMPUSES[filterCampus]);
                cards.AddRange(allCards);
                return cards;
            }
            else
            {
                List<TrelloCard> allCards = this._trelloRepository.GetAllCardsForBoard(TrelloConstants.CAMPUSES[filterCampus]).ToList<TrelloCard>();
                allCards = allCards.Where(c => c.IdMembers.Length > 0).ToList<TrelloCard>();
                filterCards = new List<TrelloCard>();
                allCards.ForEach(c => TrelloConstants.IsListNameEqualsTo(c.IdBoard, c.IdList, filterCards, c));
            }
            return filterCards;
        }

        public IEnumerable<Campus> GetAllCampusses()
        {
            IList<Campus> campusses = new List<Campus>();
            string[] campussesEnum = Enum.GetNames(typeof(CampusNames));
            for (int i = 0, l = campussesEnum.Length; i < l; i++)
                campusses.Add(new Campus(campussesEnum[i]));
            return campusses;
        }

        private Campus AddNewCampus(List<Campus> campuses, TrelloCard card)
        {
            Campus campus = new Campus(TrelloConstants.GetCampusNameForBoardId(card.IdBoard), card.IdBoard);
            if (campuses.Find(c => c.Name.Equals(campus.Name)) == null)
                campuses.Add(campus);
            return campus;
        }

        private TrelloList GetListInBoardByListId(string listId, string boardId)
        {
            string campusString = TrelloConstants.GetCampusNameForBoardId(boardId);
            CampusNames campus = TrelloConstants.GetCampusNameForString(campusString);
            CampusLists listEnum = TrelloConstants.CampusListsIds[campus][listId];
            string listName = StringEnum.GetStringValue(listEnum);

            return new TrelloList(Enum.GetName(typeof(CampusLists), listEnum), 
                CampusListsExtension.AsDisplayString(listEnum), listId);
        }
        #endregion
    }
}
