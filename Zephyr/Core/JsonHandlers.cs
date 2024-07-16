using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Core
{
    class JsonGetters
    {
        public string getAuthFromJson()
        {
            string currentPath = Directory.GetCurrentDirectory();
            string MainPath = Path.Combine(currentPath, @"..\..\..");
            string settingsPath = Path.Combine(MainPath, @"Config\settings.json");

            Debug.WriteLine(MainPath);
            if (File.Exists(settingsPath))
            {
                string json = File.ReadAllText(settingsPath);
                JObject settings = JObject.Parse(json);
                Debug.WriteLine(settings["auth"]);
                return (string)settings["auth"];
            }
            return "unknown";
        }
    }

    class JsonSetters
    {
        public void setAuthToJson(string auth)
        {
            string currentPath = Directory.GetCurrentDirectory();
            string MainPath = Path.Combine(currentPath, @"..\..\..");
            string settingsPath = Path.Combine(MainPath, @"Config\settings.json");

            Debug.WriteLine(MainPath);
            if (File.Exists(settingsPath))
            {
                string json = File.ReadAllText(settingsPath);
                JObject settings = JObject.Parse(json);
                settings["auth"] = auth;
                File.WriteAllText(settingsPath, settings.ToString());
                Debug.WriteLine("Updated auth to: " + auth);
            }
            else
            {
                Debug.WriteLine("Settings file not found.");
            }
        }
    }
}
