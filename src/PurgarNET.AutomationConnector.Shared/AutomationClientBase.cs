﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurgarNET.AutomationConnector.Shared.Models;

namespace PurgarNET.AutomationConnector.Shared
{
    public enum ClientType { Workflow, User}

    public abstract class AutomationClientBase
    {
        private static Dictionary<string, AutomationClientBase> _clientsCache = new Dictionary<string, AutomationClientBase>();

        private static object _syncLock = new object();

        protected static T GetClient<T>(string cacheKey, ClientType type, Func<T> createClientFunc) where T : AutomationClientBase
        {
            lock (_syncLock)
            {
                cacheKey = cacheKey + "-" + type;
                if (_clientsCache.ContainsKey(cacheKey))
                {
                    return (T)_clientsCache[cacheKey];
                }
                else
                {
                    var c = createClientFunc.Invoke();
                    c.ClientType = type;
                    _clientsCache.Add(cacheKey, c);
                    return c;
                }
            }
        }

        protected ClientType ClientType;

        public abstract Task<IEnumerable<AutomationRunbook>> GetRunbooksAsync();

        public abstract Task<AutomationRunbook> GetRunbookAsync(string runbookName);


        /*public abstract Task<Job> StartJob(Job j)
        {
            var jobId = Guid.NewGuid();
            return await SendAsync<Job>(_tenantId, Parameters.AUTOMATION_API_VERSION, $"jobs/{jobId}", Method.PUT, j);
        }*/


        /*public async Task<List<HybridRunbookWorkerGroup>> GetHybridRunbookWorkerGroupsAsync()
        {
            return await GetListAsync<HybridRunbookWorkerGroup>(_tenantId, Parameters.AUTOMATION_API_VERSION, "hybridRunbookWorkerGroups");
        }*/

        public abstract Task<IEnumerable<AutomationJob>> GetJobsAsync();

        public abstract Task<AutomationJob> GetJobAsync(Guid jobId);

        /*public async Task<string> GetJobOutput(Guid jobId)
        {
            var streams = await GetListAsync<OutputItem>(_tenantId, Parameters.AUTOMATION_API_VERSION, $"jobs/{jobId}/streams?$filter=properties/streamType eq 'Output'");
            var sb = new StringBuilder();
            streams.ForEach((x) => sb.AppendLine(x.Properties.Summary));
            return sb.ToString();
        }*/
    }
}
