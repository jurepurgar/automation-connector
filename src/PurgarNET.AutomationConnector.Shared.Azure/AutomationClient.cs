using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurgarNET.AutomationConnector.Shared.Models;
using System.Security;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Management.Automation;

namespace PurgarNET.AutomationConnector.Shared.Azure
{
    public class AutomationClient : AutomationClientBase
    {
        AzureEnvironment _environment = null;
        AzureAccount _account = null;
        AutomationManagementClient _client = null;
        string _resourceGroupName = null;
        string _automationAccountName = null;


        public AutomationClient()
        {
            _environment = AzureEnvironment.PublicEnvironments["AzureCloud"];

        }

        public void Initialize(Guid tenantId, Guid subscriptionId, string resourceGroupName, string automationAccountName, string appId, SecureString password)
        {
            var _account = new AzureAccount()
            {
                Id = appId,
                Type = AzureAccount.AccountType.ServicePrincipal
            };
            var c = AzureSession.AuthenticationFactory.Authenticate(_account, _environment, tenantId.ToString(), password, ShowDialog.Never);
        }

        private void InitializeInternal(Guid tenantId, Guid subscriptionId, string resourceGroupName, string automationAccountName)
        {
            var ctx = new AzureContext(new AzureSubscription() { Id = subscriptionId }, _account, _environment, new AzureTenant { Id = tenantId });
            _client = AzureSession.ClientFactory.CreateClient<AutomationManagementClient>(ctx, AzureEnvironment.Endpoint.ResourceManager);

            _resourceGroupName = resourceGroupName;
            _automationAccountName = automationAccountName;
        }

        public override Task<AutomationJob> GetJobAsync(Guid jobId)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<AutomationJob>> GetJobsAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<AutomationRunbook> GetRunbookAsync(string runbookName)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<AutomationRunbook>> GetRunbooksAsync()
        {
            var rbs = await _client.Runbooks.ListAsync(_resourceGroupName, _automationAccountName);


            var rb = rbs;
            return null;
        }
    }
}
