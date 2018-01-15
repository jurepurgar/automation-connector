using Microsoft.EnterpriseManagement.ConsoleFramework;
using Microsoft.EnterpriseManagement.UI.SdkDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PurgarNET.AutomationConnector.Console
{
    public class ManageConnectorTask : ConsoleCommand
    {
        public override void ExecuteCommand(IList<NavigationModelNodeBase> nodes, NavigationModelNodeTask task, ICollection<string> parameters)
        {
            base.ExecuteCommand(nodes, task, parameters);

            string nodesStr = "Nodes:";
            foreach (NavigationModelNodeBase node in nodes)
            {
                nodesStr += $"{node.DisplayName}/r/n";
            }

            string parametersStr = "Parameters:";
            foreach (var p in parameters)
            {
                parametersStr += p;
            }


            var str = $@"
{nodesStr}
Task:  {task.DisplayName}
{parametersStr}

";

            MessageBox.Show(str);
           

        }
    }
}
