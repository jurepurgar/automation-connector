using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurgarNET.AutomationConnector.Shared.Models;

namespace PurgarNET.AutomationConnector.Shared.Azure
{
    public class AutomationClient : AutomationClientBase
    {
        public AutomationClient()
        {


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

        public override Task<IEnumerable<AutomationRunbook>> GetRunbooksAsync()
        {
            throw new NotImplementedException();
        }
    }
}
