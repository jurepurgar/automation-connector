using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurgarNET.AutomationConnector.Shared
{
    public static class Parameters
    {
        public static readonly string AZURE_LOGIN_AUTHORITY = "https://login.microsoftonline.com/";

        public static readonly string AZURE_LOGIN_RESOURCE = "https://management.core.windows.net/";

        public static readonly string AZURE_API_APPID = "1950a258-227b-4e31-a9cf-717495945fc2";

        public static Uri REDIRECT_URI = new Uri("urn:ietf:wg:oauth:2.0:oob");
    }
}
