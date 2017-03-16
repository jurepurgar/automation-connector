using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PurgarNET.AutomationConnector.Shared.SMA;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var cl = new AutomationClient();

            cl.Initialize(new Uri("https://sma.kolektor.com/00000000-0000-0000-0000-000000000000/"));

            var r = await cl.GetRunbooksAsync();
        }
    }
}
