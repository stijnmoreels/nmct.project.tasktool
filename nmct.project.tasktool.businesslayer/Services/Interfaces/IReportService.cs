using nmct.project.tasktool.models;
using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Reports;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
namespace nmct.project.tasktool.businesslayer.Services.Interfaces 
{
    public interface IReportService
    {
        void AddTaskPerformance(List<TaskPerformance> task);
        void AddOrUpdateReactionRuntime(ReactionRuntime reactionRuntime);

        System.Collections.Generic.List<nmct.project.tasktool.models.TaskPerformance> GetPerformances(DateTime start, DateTime end);
        List<TrelloCampus> GetCampuses();

        ReactionRuntime GetReactionRuntimeForCardId(string cardId);
        List<ReactionRuntime> GetAllReactionRuntimesForCampus(CampusNames campus);
        List<ReactionRuntime> GetAllReactionRuntimes(DateTime start, DateTime end);
        List<ReactionRuntime> GetAllReactionRuntimes();

        void UpdateExistingReationtime(ReactionRuntime existingCard);
    }
}
