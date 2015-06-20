using nmct.project.tasktool.models;
using System;
using System.Collections.Generic;
namespace nmct.project.tasktool.businesslayer.Services.Interfaces
{
    public interface IRoomService
    {
        void AddCampusses(System.Collections.Generic.List<nmct.project.tasktool.models.Campus> campusses);
        void AddRooms(System.Collections.Generic.IList<nmct.project.tasktool.models.Room> rooms);
        List<Campus> GetCampusses();
    }
}
