using nmct.project.tasktool.models;
using System;
using System.Collections.Generic;
namespace nmct.project.tasktool.businesslayer.Services.Interfaces
{
    public interface IStaffService
    {
        void AddUsersInDatabase(System.Collections.Generic.List<nmct.project.tasktool.models.StaffRole> staff);
        void DeleteUsersInDatabase(System.Collections.Generic.List<nmct.project.tasktool.models.StaffRole> staff);
        System.Collections.Generic.List<nmct.project.tasktool.models.StaffRole> GetUsersInDatabase();
        Role GetRole(String name);
        StaffRole GetUserForUsername(string username);
        StaffRole GetUserForTrelloUsername(string trellousername);
        Role GetRoleFromStaff(String StaffName);
        List<Role> GetRoles();
        void UpdateStaffRoles(List<StaffRole> staff);
        void AddExternalUser(ExternalUser extUser);
        void DeleteExternalUser(int id);
        void UpdateExternalUser(List<ExternalUser> extUser);
        ExternalUser GetExternalUser(int id);
        List<ExternalUser> GetExternalUsers();
        String ChangeExternalUserToEmail(ExternalUser external);
    }
}
