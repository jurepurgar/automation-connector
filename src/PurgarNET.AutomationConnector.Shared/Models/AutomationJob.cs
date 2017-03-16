using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurgarNET.AutomationConnector.Shared.Models
{
    public class AutomationJob
    {
        public AutomationJob()
        {
            Parameters = new Dictionary<string, object>();
        }

        public string Id { get; set; }

        public Dictionary<string, object> Parameters { get; set; }

    }

}
