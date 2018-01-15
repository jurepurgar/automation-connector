using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurgarNET.AutomationConnector.Console
{
    public class Property
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }

    public abstract class ConnectorViewModelBase
    {
        public ConnectorViewModelBase(bool isNew)
        {

        }

        public ObservableCollection<Property> Properties = new ObservableCollection<Property>();

    }
}
