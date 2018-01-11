using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurgarNET.AutomationConnector.Shared.Models;
using System.Security;
using Microsoft.Azure.Management.Automation;
using Microsoft.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PurgarNET.AutomationConnector.Shared.Azure
{
    public class AzureAutomationClient : AutomationClientBase
    {
        AutomationManagementClient _client = null;
        string _resourceGroupName = null;
        string _automationAccountName = null;
        
        public AzureAutomationClient()
        {
            
        }

        public void Initialize(Guid tenantId, Guid subscriptionId, string resourceGroupName, string automationAccountName, string appId, SecureString password)
        {
            /*var _account = new AzureAccount()
            {
                Id = appId,
                Type = AzureAccount.AccountType.ServicePrincipal
            };
            var c = AzureSession.AuthenticationFactory.Authenticate(_account, _environment, tenantId.ToString(), password, ShowDialog.Never);
            */
        }

        public async Task InitializeForWorkflowAsync(Guid tenantId, Guid subscriptionId, string resourceGroupName, string automationAccountName, string appId, SecureString password)
        {
            AuthenticationContext _authCtx = new AuthenticationContext($"https://login.microsoftonline.com/{tenantId}/");
            var token = await _authCtx.AcquireTokenSilentAsync("https://management.core.windows.net/", appId);
            var c = new TokenCloudCredentials(subscriptionId.ToString(), token.AccessToken);
            
        }

        private void InitializeInternal(SubscriptionCloudCredentials cred, Guid subscriptionId, string resourceGroupName, string automationAccountName)
        {
            _client = new AutomationManagementClient(cred);

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
