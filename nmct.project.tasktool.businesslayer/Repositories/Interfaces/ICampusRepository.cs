using nmct.project.tasktool.models;
using System;
namespace nmct.project.tasktool.businesslayer.Repositories.Interfaces
{
    public interface ICampusRepository : IGenericRepository<Campus>
    {
        void AddCampus(nmct.project.tasktool.models.Campus camp);
    }
}
