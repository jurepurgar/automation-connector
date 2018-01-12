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
        private AutomationManagementClient _client = null;
        private string _resourceGroupName = null;
        private string _automationAccountName = null;
        private Guid _subscriptionId;
        private ClientCredential _clientCred = null;
        private AuthenticationContext _authCtx = null;
        private string _token = null;

        protected AzureAutomationClient(Guid tenantId, Guid subscriptionId, string resourceGroupName, string automationAccountName)
        {
            _authCtx = new AuthenticationContext(Parameters.AZURE_LOGIN_AUTHORITY + tenantId);
            _subscriptionId = subscriptionId;
            _resourceGroupName = resourceGroupName;
            _automationAccountName = automationAccountName;
            _client = new AutomationManagementClient();
        }

        private static AzureAutomationClient GetClient(ClientType type, Guid tenantId, Guid subscriptionId, string resourceGroupName, string automationAccountName)
        {
            return GetClient($"{subscriptionId}-{resourceGroupName}-{automationAccountName}", type, () => new AzureAutomationClient(tenantId, subscriptionId, resourceGroupName, automationAccountName));
        }

        public static AzureAutomationClient GetForWorkflow(Guid tenantId, Guid subscriptionId, string resourceGroupName, string automationAccountName, Guid appId, string password)
        {
            var c = GetClient(ClientType.Workflow, tenantId, subscriptionId, resourceGroupName, automationAccountName);
            c._clientCred = new ClientCredential(appId.ToString(), password);
            return c;
        }

        public static AzureAutomationClient GetForUser(Guid tenantId, Guid subscriptionId, string resourceGroupName, string automationAccountName)
        {
            return GetClient(ClientType.User, tenantId, subscriptionId, resourceGroupName, automationAccountName);
        }

        private async Task AssureLogin()
        {
            var token = await ((ClientType == ClientType.Workflow) ? GetAppToken() : GetUserToken());

            if (token != _token)
            {
                _token = token;
                _client = new AutomationManagementClient(new TokenCloudCredentials(_subscriptionId.ToString(), _token));
            }

        }

        private async Task<string> GetAppToken()
        {
            var t = await _authCtx.AcquireTokenAsync(Parameters.AZURE_LOGIN_RESOURCE, _clientCred);
            return t.AccessToken;
        }

        private async Task<string> GetUserToken()
        {
            try
            {
                return (await _authCtx.AcquireTokenSilentAsync(Parameters.AZURE_LOGIN_RESOURCE, Parameters.AZURE_API_APPID)).AccessToken;
            }
            catch
            {
                return (await _authCtx.AcquireTokenAsync(Parameters.AZURE_LOGIN_RESOURCE, Parameters.AZURE_API_APPID, Parameters.REDIRECT_URI, new PlatformParameters(PromptBehavior.SelectAccount))).AccessToken;
            }
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
            await AssureLogin();
            var rbs = await _client.Runbooks.ListAsync(_resourceGroupName, _automationAccountName);

            var rb = rbs;
            return null;
        }
    }
}
