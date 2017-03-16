using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurgarNET.AutomationConnector.Shared.Models;
using PurgarNET.AutomationConnector.Shared.SMA.SMAServiceReference;
using System.Net;

namespace PurgarNET.AutomationConnector.Shared.SMA
{
    public class AutomationClient : AutomationClientBase
    {
        private static OrchestratorApi _ctx = null;

        public AutomationClient()
        {
            
        }

        public void Initialize(Uri url, ICredentials credentials = null)
        {
            // consider moving to constructor
            _ctx = new OrchestratorApi(url);
            if (credentials == null)
            {
                credentials = CredentialCache.DefaultCredentials;
            }

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
