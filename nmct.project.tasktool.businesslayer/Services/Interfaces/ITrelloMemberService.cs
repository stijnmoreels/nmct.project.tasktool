using nmct.project.tasktool.businesslayer.Repositories.Interfaces;
using nmct.project.tasktool.models;
using nmct.project.tasktool.models.Trello;
using System;
namespace nmct.project.tasktool.businesslayer.Services.Interfaces
{
    public interface ITrelloMemberService 
    {
        TrelloMember GetMember(string trelloId);
        System.Collections.Generic.List<TrelloMember> GetTrelloMembers();
        void AddTrelloMembers(System.Collections.Generic.List<TrelloMember> mbrs);
    }
}
