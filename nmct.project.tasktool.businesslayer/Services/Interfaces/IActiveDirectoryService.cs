using nmct.project.tasktool.models;
using System;
namespace nmct.project.tasktool.businesslayer.Services.Interfaces
{
    public interface IActiveDirectoryService
    {
        System.Collections.Generic.List<StaffRole> GetMailNickNameUsers();
    }
}
