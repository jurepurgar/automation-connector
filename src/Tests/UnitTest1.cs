using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PurgarNET.AutomationConnector.Shared;
using System.Threading.Tasks;
using System.Security;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var cl = new PurgarNET.AutomationConnector.Shared.SMA.SmaAutomationClient();

            cl.Initialize(new Uri("https://sma.kolektor.com/00000000-0000-0000-0000-000000000000/"), () => null);

            var r = await cl.GetRunbooksAsync();
        }


        [TestMethod]
        public async Task TestMethod2()
        {
            var cl = new PurgarNET.AutomationConnector.Shared.Azure.AzureAutomationClient();

            var tenantId = new Guid("a600e706-e740-4516-ba18-e37812af394e");
            var subscriptionId = new Guid("5410c318-3fb7-46bd-b422-c6931e0c089c");
            var resourceGroup = "AutomationGroup";
            var accountName = "TestAuto";
            var appId = "3ee8e1f7-561e-46c6-b5f8-cd74dd64dbc8";

            var pass = "pass1";
            
            var securePass = ConvertToSecureString(pass);

            await cl.InitializeForWorkflowAsync(tenantId, subscriptionId, resourceGroup, accountName, appId, securePass);

            var r = await cl.GetRunbooksAsync();
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
