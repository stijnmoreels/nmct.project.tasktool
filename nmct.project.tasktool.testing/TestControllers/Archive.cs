using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using nmct.project.tasktool.testing.DependInjection;
using nmct.project.tasktool.models.Constants;
using System.Collections.Generic;
using nmct.project.tasktool.models.Archive;
using Newtonsoft.Json;

namespace nmct.project.tasktool.testing.TestControllers
{
    [TestClass]
    public class UnitTest1 : TestBase
    {

        //[TestMethod]
        public void TestGetArchiveTaken()
        {
            HttpResponseMessage result = this._archiveApiController.Get("GKG");
            List<TrelloArchiveCard> tasks = new List<TrelloArchiveCard>();

            System.Threading.Tasks.Task.Run(async () =>
            {
                string json = await result.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<TrelloArchiveCard>>(json);
                Assert.IsTrue(tasks.Count > 0);
                Assert.IsInstanceOfType(tasks, typeof(List<TrelloArchiveCard>));

            }).GetAwaiter().GetResult();
        }

        
    }
}
