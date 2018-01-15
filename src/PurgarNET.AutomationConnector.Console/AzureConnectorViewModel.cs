using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurgarNET.AutomationConnector.Console
{
    public class AzureConnectorViewModel : ConnectorViewModelBase
    {
        public AzureConnectorViewModel(bool isNew) : base(isNew)
        {
            Properties.Add(new Property() { Name = "TenantId", DisplayName = "Tenant ID" });
            Properties.Add(new Property() { Name = "SubscriptionId", DisplayName = "Subscription ID" });
            Properties.Add(new Property() { Name = "ResourceGroupName", DisplayName = "Resource Group Name" });
            Properties.Add(new Property() { Name = "AutomationAccountName", DisplayName = "Automation Account Name ID" });
            Properties.Add(new Property() { Name = "AppId", DisplayName = "Service principal ID" });
            Properties.Add(new Property() { Name = "Pass", DisplayName = "Service principal key" });
        }
    }
}
