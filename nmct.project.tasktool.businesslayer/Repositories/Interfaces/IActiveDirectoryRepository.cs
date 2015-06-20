using nmct.project.tasktool.models;
using System;
using System.Collections.Generic;
namespace nmct.project.tasktool.businesslayer.Repositories.Interfaces
{
    public interface IActiveDirectoryRepository
    {
        List<StaffRole> GetMailNicknameUsers(string directoryEntry, string propName, string propFullName);
    }
}
