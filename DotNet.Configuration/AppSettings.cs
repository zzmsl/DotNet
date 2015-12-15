

namespace DotNet.Configuration
{
    public class AppSettings
    {
        public static void AppSettingsSave(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) return;

            System.Configuration.Configuration cfa
                = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();

            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }

        public static string AppSettingsValue(string key)
        {
            if (string.IsNullOrEmpty(key)) return ""; 
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
