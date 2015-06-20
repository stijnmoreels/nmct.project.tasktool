using nmct.project.tasktool.models;
using System;
namespace nmct.project.tasktool.businesslayer.Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        nmct.project.tasktool.models.Role GetRoleId(string name);       
    }
}
