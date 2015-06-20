using nmct.project.tasktool.models;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
namespace nmct.project.tasktool.businesslayer.Services.TrelloCardHelper.Interfaces
{
    public interface ITrelloCardTaskCreator
    {
        nmct.project.tasktool.models.Task ConvertTrelloCardToTask(ref System.Collections.Generic.List<nmct.project.tasktool.models.Trello.TrelloMember> refMembers, System.Collections.Generic.List<nmct.project.tasktool.models.Campus> campuses, System.Collections.Generic.List<nmct.project.tasktool.models.Room> rooms, System.Collections.Generic.List<nmct.project.tasktool.models.Urgency> statuses, System.Collections.Generic.List<nmct.project.tasktool.models.Magnitude> magnitudes, nmct.project.tasktool.models.Trello.TrelloCard card);

        List<Task> ConvertTrelloCardsToTasks(List<TrelloMember> refMembers, List<Campus> campuses,
            List<Room> rooms, List<Urgency> statuses, List<Magnitude> magnitudes, List<TrelloCard> cards);
        
        nmct.project.tasktool.models.Task CreateNewTask(ref System.Collections.Generic.List<nmct.project.tasktool.models.Trello.TrelloMember> refMembers, nmct.project.tasktool.models.Trello.TrelloCard card, nmct.project.tasktool.models.Room room, nmct.project.tasktool.models.Urgency status, nmct.project.tasktool.models.Magnitude magnitude, nmct.project.tasktool.models.Campus campus);
        
        
        void FindInfoInLabels(System.Collections.Generic.List<nmct.project.tasktool.models.Trello.TrelloLabel> labels, ref nmct.project.tasktool.models.Room room, ref nmct.project.tasktool.models.Urgency urgency, ref nmct.project.tasktool.models.Magnitude magnitude, int i);
    }
}
