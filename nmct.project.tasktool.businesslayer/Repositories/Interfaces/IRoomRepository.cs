using nmct.project.tasktool.models;
using System;
namespace nmct.project.tasktool.businesslayer.Repositories.Interfaces
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        void AddRoom(nmct.project.tasktool.models.Room rm);
    }
}
