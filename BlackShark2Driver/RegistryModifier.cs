using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace XOutput.Tools
{
    public sealed class RegistryModifier
    {
        /// <summary>
        /// Startup registry key.
        /// </summary>
        public const string AutostartRegistry = @"Software\Microsoft\Windows\CurrentVersion\Run";
        /// <summary>
        /// XOutput registry value
        /// </summary>
        public const string AutostartValueKey = "XOutput";
        /// <summary>
        /// Autostart command line parameters.
        /// </summary>
        public const string AutostartParams = " --minimized";

        //private static readonly ILogger logger = LoggerFactory.GetLogger(typeof(RegistryModifier));

        /// <summary>
        /// Gets or sets the autostart.
        /// </summary>
        public bool Autostart
        {
            get
            {
                using (RegistryKey key = GetRegistryKey())
                {
                    bool exists = key.GetValue(AutostartValueKey) != null;
                    Console.WriteLine($"{AutostartValueKey} registry is " + (exists ? "" : "not ") + "found");
                    return exists;
                }
            }
            set
            {
                if (value)
                {
                    SetAutostart();
                }
                else
                {
                    ClearAutostart();
                }
            }
        }

        /// <summary>
        /// Activates autostart.
        /// </summary>
        public void SetAutostart()
        {
            using (RegistryKey key = GetRegistryKey())
            {
                string filename = Process.GetCurrentProcess().MainModule.FileName;
                string value = $"\"{filename}\" {AutostartParams}";
                key.SetValue(AutostartValueKey, value);
                Console.WriteLine($"{AutostartValueKey} registry set to {value}");
            }
        }

        /// <summary>
        /// Deactivates autostart.
        /// </summary>
        public void ClearAutostart()
        {
            using (RegistryKey key = GetRegistryKey())
            {
                key.DeleteValue(AutostartValueKey);
                Console.WriteLine($"{AutostartValueKey} registry is deleted");
            }
        }

        private static RegistryKey GetRegistryKey(bool writeable = true)
        {
            return Registry.CurrentUser.OpenSubKey(AutostartRegistry, writeable);
        }

        public static bool KeyExists(RegistryKey registryKey, string subkey)
        {
            using (RegistryKey registry = registryKey.OpenSubKey(subkey))
            {
                return registry != null;
            }
        }

        public static void DeleteTree(RegistryKey registryKey, string subkey)
        {
            registryKey.DeleteSubKeyTree(subkey, false);
            registryKey.Close();
        }

        public static void CreateKey(RegistryKey registryKey, string subkey)
        {
            RegistryKey registry = registryKey.CreateSubKey(subkey);
            registry.Close();
        }

        public static object GetValue(RegistryKey registryKey, string subkey, string key)
        {
            return registryKey.OpenSubKey(subkey).GetValue(key);
        }

        public static void SetValue(RegistryKey registryKey, string subkey, string key, object value)
        {
            using (RegistryKey registry = registryKey.OpenSubKey(subkey, true))
            {
                registry.SetValue(key, value);
            }
        }

        public static string[] GetSubKeyNames(RegistryKey registryKey, string subkey)
        {
            return registryKey.OpenSubKey(subkey).GetSubKeyNames();
        }
    }
}
