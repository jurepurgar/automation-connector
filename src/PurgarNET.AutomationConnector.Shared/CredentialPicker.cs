
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PurgarNET.AutomationConnector.Shared
{
    public class CredentialPicker
    {
        [DllImport("ole32.dll")]
        private static extern void CoTaskMemFree(IntPtr ptr);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct CREDUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }

        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern bool CredUnPackAuthenticationBuffer(int dwFlags, IntPtr pAuthBuffer, uint cbAuthBuffer, StringBuilder pszUserName, ref int pcchMaxUserName, StringBuilder pszDomainName, ref int pcchMaxDomainame, StringBuilder pszPassword, ref int pcchMaxPassword);

        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO notUsedHere, int authError, ref uint authPackage, IntPtr InAuthBuffer, uint InAuthBufferSize, out IntPtr refOutAuthBuffer, out uint refOutAuthBufferSize, ref bool fSave, int flags);

        public static NetworkCredential PromptForCredential(string caption, string text)
        {
            var credui = new CREDUI_INFO
            {
                pszCaptionText = caption,
                pszMessageText = text
            };
            credui.cbSize = Marshal.SizeOf(credui);
            uint authPackage = 0;
            bool save = false;

            //const int loginErrorCode = 1326;    //Login Failed
            var authError = 0;

            var result = CredUIPromptForWindowsCredentials(ref credui, authError, ref authPackage, IntPtr.Zero, 0, out IntPtr outCredBuffer, out uint outCredSize, ref save, 1);

            var usernameBuf = new StringBuilder(100);
            var passwordBuf = new StringBuilder(100);
            var domainBuf = new StringBuilder(100);

            var maxUserName = 100;
            var maxDomain = 100;
            var maxPassword = 100;
            if (result == 0)
            {
                if (CredUnPackAuthenticationBuffer(0, outCredBuffer, outCredSize, usernameBuf, ref maxUserName, domainBuf, ref maxDomain, passwordBuf, ref maxPassword))
                {
                    CoTaskMemFree(outCredBuffer);
                    return new NetworkCredential()
                    {
                        UserName = usernameBuf.ToString(),
                        Password = passwordBuf.ToString(),
                        Domain = domainBuf.ToString()
                    };
                }
            }
            return null;
        }
    }
}
