using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models;
using nmct.project.tasktool.models.Extensions;
using nmct.project.tasktool.models.Interfaces;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using nmct.project.tasktool.businesslayer.Services.TrelloCardHelper.Interfaces;
using System.Diagnostics;

namespace nmct.project.tasktool.businesslayer.Services.TrelloCardHelper
{
    // -------------------------------------------------------------------------------------------------------------------------
    // Stijn Moreels - 10.06.2015
    // 
    // Main Reason: Creation of new Tasks from Cards
    // -------------------------------------------------------------------------------------------------------------------------
    public class TrelloCardTaskCreator : ITrelloCardTaskCreator
    {
        private ITrelloCardInfoFinder _trelloCardInfoFinder = null;

        public TrelloCardTaskCreator(ITrelloCardInfoFinder trelloCardInfoFinder)
        {
            this._trelloCardInfoFinder = trelloCardInfoFinder;
        }

        // ----------------------------------------------------------------------------------------------------
        // TrelloCards > Tasks
        // ----------------------------------------------------------------------------------------------------
        public List<Task> ConvertTrelloCardsToTasks(List<TrelloMember> refMembers, List<Campus> campuses,
            List<Room> rooms, List<Urgency> statuses, List<Magnitude> magnitudes, List<TrelloCard> cards)
        {
            List<Task> tasks = new List<Task>();
            foreach (TrelloCard card in cards)
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                Room room = null;
                Urgency status = null;
                Magnitude magnitude = null;
                AddNewRoomOrStatus(rooms, statuses, magnitudes, card, ref room, ref status, ref magnitude);

                Campus campus = AddNewCampus(campuses, card);
                Task task = CreateNewTask(ref refMembers, card, room, status, magnitude, campus);
                watch.Stop();
                tasks.Add(task);
            }
            return tasks;
        }

        // ----------------------------------------------------------------------------------------------------
        // TrelloCard > Task
        // ----------------------------------------------------------------------------------------------------
        public Task ConvertTrelloCardToTask(ref List<TrelloMember> refMembers, List<Campus> campuses,
            List<Room> rooms, List<Urgency> statuses, List<Magnitude> magnitudes, TrelloCard card)
        {
            Task task = new Task();
            Room room = null;
            Urgency status = null;
            Magnitude magnitude = null;
            AddNewRoomOrStatus(rooms, statuses, magnitudes, card, ref room, ref status, ref magnitude);

            Campus campus = AddNewCampus(campuses, card);
            return CreateNewTask(ref refMembers, card, room, status, magnitude, campus);
        }

        private void AddNewRoomOrStatus(List<Room> rooms, List<Urgency> statuses, List<Magnitude> magnitudes,
            TrelloCard card, ref Room room, ref Urgency status, ref Magnitude magnitude)
        {
            GetRoomAndStatusFromLabels(card.Labels, ref room, ref status, ref magnitude);
            AddNewItemToList<Room>(room, rooms);
            AddNewItemToList<Urgency>(status, statuses);
            AddNewItemToList<Magnitude>(magnitude, magnitudes);
        }

        private void GetRoomAndStatusFromLabels(List<TrelloLabel> labels, ref Room room,
            ref Urgency status, ref Magnitude magnitude)
        {
            for (int i = 0, l = labels.Count; i < l; i++)
                FindInfoInLabels(labels, ref room, ref status, ref magnitude, i);
        }

        public void FindInfoInLabels(List<TrelloLabel> labels, ref Room room, ref Urgency urgency, ref Magnitude magnitude, int i)
        {
            if (labels[i].Color == null)
                room = LabelIsRoom(labels, room, i);    // ROOM
            else if (labels[i].Color.Equals("black"))
                magnitude = LabelIsMagnitude(labels, magnitude, i); // MAGNITUDE
            else if (labels[i].Color != null)
                urgency = LabelIsUrgency(labels, urgency, i); // URGENCY
        }

        // ----------------------------------------------------------------
        // ROOM
        // ----------------------------------------------------------------
        private Room LabelIsRoom(List<TrelloLabel> labels, Room room, int i)
        {
            TrelloLabel label = labels[i];
            room = new Room(label.Id, label.Name, label.IdBoard);
            return room;
        }

        // ----------------------------------------------------------------
        // Magnitude
        // ----------------------------------------------------------------
        private Magnitude LabelIsMagnitude(List<TrelloLabel> labels, Magnitude magnitude, int i)
        {
            TrelloLabel label = labels[i];
            Regex re = new Regex(@"\d+");
            Match m = re.Match(label.Name);
            if (m.Success)
                magnitude = new Magnitude(label.Id, label.Name, int.Parse(m.Value));
            else
                magnitude = new Magnitude(label.Id, label.Name);
            return magnitude;
        }

        // ----------------------------------------------------------------
        // Urgency
        // ----------------------------------------------------------------
        private Urgency LabelIsUrgency(List<TrelloLabel> labels, Urgency urgency, int i)
        {
            TrelloLabel label = labels[i];
            string urgencyShort = Regex.Replace(label.Name, @"\s+", String.Empty);
            urgency = new Urgency(urgencyShort, UrgencyExtension.AsDisplayString(urgencyShort), label.Id);
            return urgency;
        }

        private void AddNewItemToList<T>(T item, List<T> items) where T : ILabel, new()
        {
            if (item != null)
            {
                T find = new T();
                find.Name = item.Name;
                if (items.Find(i => i.Name.Equals(find.Name)) == null)
                    items.Add(item);
            }
        }

        private Campus AddNewCampus(List<Campus> campuses, TrelloCard card)
        {
            Campus campus = new Campus(TrelloConstants.GetCampusNameForBoardId(card.IdBoard), card.IdBoard);
            if (campuses.Find(c => c.Name.Equals(campus.Name)) == null)
                campuses.Add(campus);
            return campus;
        }

        public Task CreateNewTask(ref List<TrelloMember> refMembers, TrelloCard card,
            Room room, Urgency status, Magnitude magnitude, Campus campus)
        {
            Task task = new Task();

            // BASIC INFO
            task.Name = card.Name;
            task.Description = card.Desc;
            task.List = this._trelloCardInfoFinder.GetListInBoardByListId(card.IdList, card.IdBoard);
            task.Campus = campus;

            // DATES
            task.DueDate = card.Due != null ?
                card.Due.Value.ToString(CultureInfo.CurrentCulture) : null;
            task.CreationDate = TrelloCardIdConverter.GetDatetimeFromCardId(card.Id).ToString(CultureConstants.DATETIME_FORMAT);

            // LABELS
            task.Room = room;
            task.Magnitude = magnitude;
            task.Urgency = status;

            // ID'S FROM TRELLO
            task.BoardId = card.IdBoard;
            task.CardId = card.Id;
            task.MemberIds = card.IdMembers;
            
            // ATTACHMENTS
            task.AttachmentsCount = card.Badges.Attachments;
 
            // COMMENTS
            task.CommentsCount = card.Badges.Comments;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            // MEMBERS
            task.Members = this._trelloCardInfoFinder.FindAllMembersForCard(card.IdMembers, ref refMembers).ToList<TrelloMember>();
            task.MemberCount = card.IdMembers.Length;
            watch.Stop();

            return task;
        }
    }
}
