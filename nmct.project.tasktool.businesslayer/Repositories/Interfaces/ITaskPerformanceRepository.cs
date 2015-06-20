using nmct.project.tasktool.models;
using System;
namespace nmct.project.tasktool.businesslayer.Repositories.Interfaces
{
    public interface ITaskPerformanceRepository : IGenericRepository<TaskPerformance>
    {
        System.Collections.Generic.List<nmct.project.tasktool.models.TaskPerformance> GetTaskPerformance(DateTime startDate, DateTime endDate);
    }
}
