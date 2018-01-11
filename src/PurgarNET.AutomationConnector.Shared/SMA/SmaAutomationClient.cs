using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurgarNET.AutomationConnector.Shared.Models;
using System.Net;
using Orchestrator.ResourceModel;

namespace PurgarNET.AutomationConnector.Shared.SMA
{
    public class SmaAutomationClient : AutomationClientBase
    {
        private static OrchestratorApi _ctx = null;

        public void Initialize(Uri url, Func<ICredentials> getCredFunc)
        {
            var creds = getCredFunc(); // check if is win integrated auth
            if (creds == null)
            {
                creds = CredentialCache.DefaultCredentials;
            }

            Initialize(url, creds);
        }

        public void Initialize(Uri url, ICredentials credentials)
        {
            _ctx = new OrchestratorApi(url);
            (_ctx as System.Data.Services.Client.DataServiceContext).Credentials = credentials;
        }

        public override Task<AutomationRunbook> GetRunbookAsync(string runbookName)
        {
            return Task<AutomationRunbook>.Factory.StartNew(() => 
            {
                var runbook = _ctx.Runbooks.Expand(x => x.PublishedRunbookVersion).FirstOrDefault(x => x.RunbookName == runbookName);
                return ToAutomationRunbook(runbook);
            });
        }

        public override Task<IEnumerable<AutomationRunbook>> GetRunbooksAsync()
        {
            return Task<IEnumerable<AutomationRunbook>>.Factory.StartNew(() =>
            {
                var runbooks = _ctx.Runbooks.Expand(x => x.PublishedRunbookVersion).ToList().Select(x => ToAutomationRunbook(x)).ToList();
                return runbooks;
            });
        }

        public override Task<AutomationJob> GetJobAsync(Guid jobId)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<AutomationJob>> GetJobsAsync()
        {
            throw new NotImplementedException();
        }

        private AutomationRunbook ToAutomationRunbook(Runbook runbook)
        {
            var typeStr = runbook.RunbookType.ToLower();
            RunbookType type = RunbookType.Unknown;
            if (typeStr == "script")
            {
                type = RunbookType.Workflow;
            }
            else if (typeStr == "powershellscript")
            {
                type = RunbookType.Script;
            }

            var rb = new AutomationRunbook()
            {
                Name = runbook.RunbookName,
                RunbookType = type
                
            };
            return rb;
        }

        
    }
}
