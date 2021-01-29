using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SentSpeed.lib
{
    public static class RegistryAutorun
    {
        public static string AppName
        {
            get; set;
        } = "SentSpeed";
        public static string Path
        {
            get; set;
        } = @"E:\SentSpeed-master\SentSpeed\bin\Release\SentSpeed.exe";
        private static RegistryKey RegAutorun = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static bool isItem()
        {           
            return RegAutorun.GetValue(AppName)==null?false:true;
        }
        public static bool IsAdm()
        {
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {             
                isElevated = new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
            }
            return isElevated;
        }
        public static void add()
        {
            if (!isItem())
                RegAutorun.SetValue(AppName, Path);
        }
        public static void remove()
        {
            if (isItem())
                RegAutorun.DeleteValue(AppName, false);
        }
    }
}
