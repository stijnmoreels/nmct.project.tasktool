using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nmct.project.tasktool.web.Controllers;
using nmct.project.tasktool.businesslayer.Services;
using nmct.project.tasktool.businesslayer.Repositories.Interfaces;
using nmct.project.tasktool.businesslayer.Services.Interfaces;
using nmct.project.tasktool.businesslayer.Repositories;
using nmct.project.tasktool.models.Trello;
using nmct.project.tasktool.businesslayer.Context;
using nmct.project.tasktool.models;
using System.Web.Mvc;
using nmct.project.tasktool.web.ViewModels;
using System.Collections.Generic;
using nmct.project.tasktool.testing.DependInjection;

namespace nmct.project.tasktool.testing.TestControllers
{
    [TestClass]
    public class Settings : TestBase
    {
        [TestMethod]
        public void TestShowAllUsers()
        {
            var result = this._settingsController.ShowUsers() as PartialViewResult;
            Assert.AreEqual(typeof(SettingsVM), result.Model.GetType());
            SettingsVM set = result.Model as SettingsVM;
            Assert.IsTrue(set.Roles.Count == 4);
            Assert.IsTrue(set.Staff.Count > 0);
        }
        [TestMethod]
        public void TestGiveAllExternalUser()
        {
            var result = this._settingsController.ShowExternalUsers() as PartialViewResult;
            Assert.AreEqual(typeof(List<ExternalUser>), result.Model.GetType());
            List<ExternalUser> externalusers = result.Model as List<ExternalUser>;
            Assert.IsTrue(externalusers.Count >= 0);
        }
        [TestMethod]
        public void TestAddExternalUser()
        {
            var result = this._settingsController.AddExternalUser() as PartialViewResult;
            Assert.AreEqual(typeof(ExternalUser), result.Model.GetType());
        }
    }
}
