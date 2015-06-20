using nmct.project.tasktool.models;
using System;
namespace nmct.project.tasktool.businesslayer.Repositories.Interfaces
{
    public interface IStaffRepository : IGenericRepository<StaffRole>
    {
        nmct.project.tasktool.models.StaffRole getStaff(string username);
    }
}
