using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PurgarNET.AutomationConnector.Shared;
using System.Threading.Tasks;
using System.Security;
using PurgarNET.AutomationConnector.Shared.Azure;
using PurgarNET.AutomationConnector.Shared.SMA;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestSmaUser()
        {
            var cl = SmaClient.GetForUser(new Uri("https://sma.kolektor.com/00000000-0000-0000-0000-000000000000/"));
            var r = await cl.GetRunbooksAsync();
        }

        [TestMethod]
        public async Task TestSmaWorkflow()
        {
            var cl = SmaClient.GetForWorkflow(new Uri("https://sma.kolektor.com/00000000-0000-0000-0000-000000000000/"), "testuser", "testpass");
            var r = await cl.GetRunbooksAsync();
        }


        [TestMethod]
        public async Task TestAzureWorkflow()
        {
            var tenantId = new Guid("a600e706-e740-4516-ba18-e37812af394e");
            var subscriptionId = new Guid("5410c318-3fb7-46bd-b422-c6931e0c089c");
            var resourceGroup = "AutomationGroup";
            var accountName = "TestAuto";
            var appId = new Guid("3ee8e1f7-561e-46c6-b5f8-cd74dd64dbc8");

            var pass = "";


            var cl = AzureAutomationClient.GetForWorkflow(tenantId, subscriptionId, resourceGroup, accountName, appId, pass);

            var r = await cl.GetRunbooksAsync();

            r = await cl.GetRunbooksAsync();

            r = await cl.GetRunbooksAsync();
        }

        [TestMethod]
        public async Task TestAzureUser()
        {
            var tenantId = new Guid("a600e706-e740-4516-ba18-e37812af394e");
            var subscriptionId = new Guid("5410c318-3fb7-46bd-b422-c6931e0c089c");
            var resourceGroup = "AutomationGroup";
            var accountName = "TestAuto";

            var cl = AzureAutomationClient.GetForUser(tenantId, subscriptionId, resourceGroup, accountName);

            var r = await cl.GetRunbooksAsync();

            r = await cl.GetRunbooksAsync();

            r = await cl.GetRunbooksAsync();
        }

        private static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }
    }
}
