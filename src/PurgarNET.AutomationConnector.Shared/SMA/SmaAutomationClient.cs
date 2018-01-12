using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurgarNET.AutomationConnector.Shared.Models;
using System.Net;
using Orchestrator.ResourceModel;
using System.Data.Services.Client;

namespace PurgarNET.AutomationConnector.Shared.SMA
{
    public class SmaClient : AutomationClientBase
    {
        private OrchestratorApi _ctx = null;

        public SmaClient(OrchestratorApi ctx)
        {
            _ctx = ctx;
        }

        public static SmaClient GetForWorkflow(Uri url, string username, string password)
        {
            return GetClient(url.ToString(), ClientType.Workflow, () =>
            {
                var ctx = new OrchestratorApi(url);
                (ctx as DataServiceContext).Credentials = new NetworkCredential(username, password);
                return new SmaClient(ctx);
            });
        }

        public static SmaClient GetForUser(Uri url)
        {
            return GetClient(url.ToString(), ClientType.Workflow, () =>
            {
                var ctx = new OrchestratorApi(url);
                (ctx as DataServiceContext).Credentials = CredentialCache.DefaultNetworkCredentials;
                return new SmaClient(ctx);
            });
        }

        public override Task<AutomationRunbook> GetRunbookAsync(string runbookName)
        {
            return Task<AutomationRunbook>.Factory.StartNew(() => 
            {
                var runbook = _ctx.Runbooks.Expand(x => x.PublishedRunbookVersion).FirstOrDefault(x => x.RunbookName == runbookName);
                return ToAutomationRunbook(runbook);
            });
        }

        public override async Task<IEnumerable<AutomationRunbook>> GetRunbooksAsync()
        {
            return await ExecuteCommand(() => _ctx.Runbooks.Expand(x => x.PublishedRunbookVersion).ToList().Select(x => ToAutomationRunbook(x)).ToList());
        }

        public override Task<AutomationJob> GetJobAsync(Guid jobId)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<AutomationJob>> GetJobsAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<T> ExecuteCommand<T>(Func<T> command)
        {
            var retry = false;
            var count = 0;
            T result = default(T);

            do
            {
                count++;
                try
                {
                    result = await Task<T>.Factory.StartNew(() => command.Invoke());
                }
                catch (Exception e)
                {
                    if (ClientType == ClientType.User && e.InnerException != null && e.InnerException is DataServiceClientException ie && (ie.StatusCode == 401 || ie.StatusCode == 403) && count < 5)
                    {
                        retry = true;
                        var c = CredentialPicker.PromptForCredential("test", "test");
                        if (c != null)
                        {
                            (_ctx as DataServiceContext).Credentials = c;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            while (retry);

            return result;
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
