using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurgarNET.AutomationConnector.Shared.Models
{
    public enum RunbookState { New, Published, Edit }
    public enum RunbookType { Workflow, Script, Graphical, Unknown }

    public class AutomationRunbook
    {
        public AutomationRunbook()
        {
            Parameters = new Dictionary<string, RunbookParameter>();
        }

        public string Name { get; set; }

        public RunbookType RunbookType { get; set; }

        public RunbookState State { get; set; }

        public Dictionary<string, RunbookParameter> Parameters { get; set; }
    }

    public class RunbookParameter
    {
        public string Type { get; set; }

        public bool IsMandatory { get; set; }

        public object DefaultValue { get; set; }
    }
    
}
