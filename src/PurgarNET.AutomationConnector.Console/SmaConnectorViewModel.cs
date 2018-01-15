using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurgarNET.AutomationConnector.Console
{
    public class SmaConnectorViewModel : ConnectorViewModelBase
    {
        public SmaConnectorViewModel(bool isNew) : base(isNew)
        {
            Properties.Add(new Property() { Name = "Url", DisplayName = "Url" });
            Properties.Add(new Property() { Name = "Username", DisplayName = "Username" });
            Properties.Add(new Property() { Name = "password", DisplayName = "Password" });
        }
    }
}
