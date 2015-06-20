using nmct.project.tasktool.models.Reports;
using System;
using System.Collections.Generic;
namespace nmct.project.tasktool.businesslayer.Repositories.Interfaces
{
    public interface IReactionRuntimeRepository : IGenericRepository<ReactionRuntime>
    {
        ReactionRuntime GetByCardId(string cardId);
        IEnumerable<ReactionRuntime> GetAllForCampusId(string campusId);
        IEnumerable<ReactionRuntime> GetAllTaskReactionRuntime(DateTime startDate, DateTime endDate);
    }
}
