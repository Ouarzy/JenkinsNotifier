namespace Jenkins.Notifier.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using Jenkins.Notifier.Model;

    public class FileSettings : IFileSettings
    {
        private static readonly string SettingsFolderName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/JenkinsNotifier/";
        private const string SettingsFileName = "SettingsJenkins.settings";
        private static readonly string SettingsFullFileName = SettingsFolderName + SettingsFileName;

        public SerializableSettings Load()
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(SettingsFullFileName);
                var serializer = new XmlSerializer(typeof(SerializableSettings));
                return (SerializableSettings)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void Save(string urlServer, IList<JenkinsViewModel> jenkinsViews, string login, string apitoken, int refreshDelay)
        {
            StreamWriter writer = null;

            try
            {
                this.CreateSettingFolderIfNotExists();
                writer = new StreamWriter(SettingsFullFileName, false);
                var serializableSettings = new SerializableSettings
                {
                    UrlServer = urlServer,
                    JenkinsViews = jenkinsViews.ToList(),
                    Login = login,
                    Apitoken = apitoken,
                    RefreshDelay = refreshDelay
                };
                var serializer = new XmlSerializer(serializableSettings.GetType());
                serializer.Serialize(writer, serializableSettings);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private void CreateSettingFolderIfNotExists()
        {
            if (!Directory.Exists(SettingsFolderName))
            {
                Directory.CreateDirectory(SettingsFolderName);                
            }
        }
    }
}
