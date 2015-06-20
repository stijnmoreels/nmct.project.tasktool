using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrelloNet;
using System.Configuration;
using nmct.project.tasktool.businesslayer.Repositories.Interfaces;
using nmct.project.tasktool.businesslayer.Repositories;
using nmct.project.tasktool.businesslayer.Services.Interfaces;
using nmct.project.tasktool.businesslayer.Services;
using nmct.project.tasktool.models;
using nmct.project.tasktool.businesslayer.Context;
using nmct.project.tasktool.models.Trello;
using nmct.project.tasktool.web.Controllers.Api;
using System.Net.Http;
using Newtonsoft.Json;
using nmct.project.tasktool.web.ViewModels;
using System.Collections.Generic;
using nmct.project.tasktool.testing.DependInjection;
using System.Web.Mvc;

namespace nmct.project.tasktool.testing.TestControllers
{
    [TestClass]
    public class Overview : TestBase
    {
        public Member me;
        public ITrello _trello;
        private string _appkey = "appkey", _token = "token";

        [TestInitialize]
        public void TestRepositoryInit()
        {
            _trello = new Trello(ConfigurationManager.AppSettings[_appkey]); // APP SETTINGS
            _trello.Authorize(ConfigurationManager.AppSettings[_token]); // APP SETTINGS
            me = _trello.Members.Me();
        }

       [TestMethod]
        public async void TestOverview()
        { 
            HttpResponseMessage message = await this._overviewApiController.Get("GKG");
            OverviewVM vm = new OverviewVM();

            System.Threading.Tasks.Task.Run(async () =>
            {
                string json = await message.Content.ReadAsStringAsync();
                vm = JsonConvert.DeserializeObject<OverviewVM>(json);

                Assert.IsInstanceOfType(vm.Tasks, typeof(IList<Task>));
                Assert.IsTrue(vm.Campuses.Count == 5);
                Assert.IsTrue(vm.Tasks.Count > 0);
            }).GetAwaiter().GetResult();
       
        }

         [TestMethod]
        public void GetAllUsers()
        {
            List<StaffRole> stf = _staffService.GetUsersInDatabase();
            Assert.IsNotNull(stf);

        }
        [TestMethod]
        public void TestGetOneTask()
        {
            //ViewResult result = this._overviewController.Task("55782aaf9c8fda53e33571d7") as ViewResult;
            //Assert.IsInstanceOfType(result.Model, typeof(Task));
            //Assert.IsNotNull(result.Model);

            ViewResult result2 = this._overviewController.Task("invalidId") as ViewResult;
            Assert.IsNull(result2.Model);

        }
    }
}
