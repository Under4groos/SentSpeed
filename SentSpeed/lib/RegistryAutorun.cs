using Microsoft.Win32;
using System.Security.Principal;

namespace SentSpeed.lib
{
    public static class RegistryAutorun
    {
        /// <summary>
        /// Имя в реестре 
        /// </summary>
        public static string AppName
        {
            get; set;
        } = "SentSpeed";

        /// <summary>
        /// Путь к нашему EXE - ку. 
        /// </summary>
        public static string Path
        {
            get; set;
        } = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        private static RegistryKey RegAutorun = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static bool isItem()
        {           
            return RegAutorun.GetValue(AppName)==null?false:true;
        }
        /// <summary>
        /// Проверка на запуск от им. Адм
        /// </summary>
        /// <returns></returns>
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
