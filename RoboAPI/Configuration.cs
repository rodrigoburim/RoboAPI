using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboAPI
{
    public static class Configuration
    {
        static Configuration()
        {
            AppSettingsReader appSettingsReader = new AppSettingsReader();

            BaseUri = (string)appSettingsReader.GetValue("BaseUri", typeof(string));
            RelativeGetUri = (string)appSettingsReader.GetValue("GetAPIRelativeUri", typeof(string));
            RelativeDeleteUri = (string)appSettingsReader.GetValue("DeleteAPIRelativeUri", typeof(string));

            Headers = new Dictionary<string, string>();

            Headers.Add("ClientIDName", (string)appSettingsReader.GetValue("ClientIDName", typeof(string)));
            Headers.Add("ClientIDValue", (string)appSettingsReader.GetValue("ClientIDValue", typeof(string)));
            Headers.Add("ClientSecretName", (string)appSettingsReader.GetValue("ClientSecretName", typeof(string)));
            Headers.Add("ClientSecretValue", (string)appSettingsReader.GetValue("ClientSecretValue", typeof(string)));
        }

        static public string BaseUri { get; set; }
        static public string RelativeGetUri { get; set; }
        static public string RelativeDeleteUri { get; set; }

        static public Dictionary<string, string> Headers { get; set; }
    }
}
