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
            Properties.Add(new Property() { DisplayName = "Url" });
            Properties.Add(new Property() { DisplayName = "Username" });
            Properties.Add(new Property() { DisplayName = "Password" });
        }
    }
}
