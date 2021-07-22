<<<<<<< HEAD
﻿using Microsoft.Win32;
using System;
=======
﻿using System;
using Microsoft.Win32;
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
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
<<<<<<< HEAD
                using (RegistryKey key = GetRegistryKey())
=======
                using (var key = GetRegistryKey())
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
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
<<<<<<< HEAD
            using (RegistryKey key = GetRegistryKey())
            {
                string filename = Process.GetCurrentProcess().MainModule.FileName;
=======
            using (var key = GetRegistryKey())
            {
                var filename = Process.GetCurrentProcess().MainModule.FileName;
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
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
<<<<<<< HEAD
            using (RegistryKey key = GetRegistryKey())
=======
            using (var key = GetRegistryKey())
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
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
<<<<<<< HEAD
            using (RegistryKey registry = registryKey.OpenSubKey(subkey))
=======
            using (var registry = registryKey.OpenSubKey(subkey))
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
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
<<<<<<< HEAD
            RegistryKey registry = registryKey.CreateSubKey(subkey);
=======
            var registry = registryKey.CreateSubKey(subkey);
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
            registry.Close();
        }

        public static object GetValue(RegistryKey registryKey, string subkey, string key)
        {
            return registryKey.OpenSubKey(subkey).GetValue(key);
        }

        public static void SetValue(RegistryKey registryKey, string subkey, string key, object value)
        {
<<<<<<< HEAD
            using (RegistryKey registry = registryKey.OpenSubKey(subkey, true))
=======
            using (var registry = registryKey.OpenSubKey(subkey, true))
>>>>>>> 713ec09460ab795f2f5676c92c9d7cb6bb0e7f09
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
