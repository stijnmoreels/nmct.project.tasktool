using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Collections.Generic;
using nmct.project.tasktool.models;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Linq;
using nmct.project.tasktool.models.Reports;
using nmct.project.tasktool.testing.DependInjection;

namespace nmct.project.tasktool.testing.TestControllers
{
    [TestClass]
    public class Reports : TestBase
    {
        [TestMethod]
        public void TestGetMethodWithoutStartAndEndTime()
        {
            HttpResponseMessage result = this._reportsApiController.Get();
            List<TaskPerformance> tasks = new List<TaskPerformance>();

            System.Threading.Tasks.Task.Run(async () =>
            {
                string json = await result.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<TaskPerformance>>(json);
                Assert.IsTrue(tasks.Count > 0);
                Assert.IsInstanceOfType(tasks, typeof(List<TaskPerformance>));
               
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void TestGetMethodWithStartAndEndTime1()
        {
            HttpResponseMessage result = this._reportsApiController.Get(DateTime.Now, DateTime.Now.AddDays(1));
            List<TaskPerformance> tasks = new List<TaskPerformance>();

            System.Threading.Tasks.Task.Run(async () =>
            {
                string json = await result.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<TaskPerformance>>(json);
                Assert.IsTrue(tasks.Count == 0);
                Assert.IsInstanceOfType(tasks, typeof(List<TaskPerformance>));

            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void TestGetMethodWithStartAndEndTime2()
        {
            HttpResponseMessage result = this._reportsApiController.Get(DateTime.Now, DateTime.Now.AddDays(-10));
            List<TaskPerformance> tasks = new List<TaskPerformance>();

            System.Threading.Tasks.Task.Run(async () =>
            {
                string json = await result.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<TaskPerformance>>(json);
                int aantal = (from t in tasks where (DbFunctions.TruncateTime(t.Date) >= DateTime.Now) && (DbFunctions.TruncateTime(t.Date) <= DateTime.Now.AddDays(-10)) select t).ToList<TaskPerformance>().Count();
                Assert.IsTrue(tasks.Count == aantal );
                Assert.IsInstanceOfType(tasks, typeof(List<TaskPerformance>));

            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestGetRuntimeMethodWithoutStartAndEndTime()
        {
            HttpResponseMessage result = this._reportsApiController.GetRuntime(DateTime.Now, DateTime.Now.AddDays(-10));
            List<ReactionRuntime> tasks = new List<ReactionRuntime>();

            System.Threading.Tasks.Task.Run(async () =>
            {
                string json = await result.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<ReactionRuntime>>(json);
                Assert.IsTrue(tasks.Count == 0);
                Assert.IsInstanceOfType(tasks, typeof(List<ReactionRuntime>));

            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void TestGetRuntimeMethodWithStartAndEndTime1()
        {
            HttpResponseMessage result = this._reportsApiController.GetRuntime(DateTime.Now, DateTime.Now.AddDays(1));
            List<ReactionRuntime> tasks = new List<ReactionRuntime>();

            System.Threading.Tasks.Task.Run(async () =>
            {
                string json = await result.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<ReactionRuntime>>(json);
                Assert.IsTrue(tasks.Count == 0);
                Assert.IsInstanceOfType(tasks, typeof(List<ReactionRuntime>));

            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestGetRuntimeMethodWithStartAndEndTime2()
        {
            HttpResponseMessage result = this._reportsApiController.GetRuntime(DateTime.Now, DateTime.Now.AddDays(-10));
            List<ReactionRuntime> tasks = new List<ReactionRuntime>();

            System.Threading.Tasks.Task.Run(async () =>
        {
                string json = await result.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<ReactionRuntime>>(json);
                int aantal = (from t in tasks where (DbFunctions.TruncateTime(t.CardIsDone) >= DateTime.Now) && (DbFunctions.TruncateTime(t.CardIsDone) <= DateTime.Now.AddDays(-10)) select t).ToList<ReactionRuntime>().Count();
                Assert.IsTrue(tasks.Count == aantal);
                Assert.IsInstanceOfType(tasks, typeof(List<ReactionRuntime>));

            }).GetAwaiter().GetResult();
        }
    }
}
