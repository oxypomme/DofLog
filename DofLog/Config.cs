using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DofLog
{
    [Serializable()]
    public class Config
    {
        #region Public Fields

        public Guid UUID { get; private set; }
        public string AL_Path { get; set; }
        public bool StayLog { get; set; }
        public bool RetroMode { get; set; }

        public bool DiscordEnabled { get; set; }

        public bool AutoOrganizer { get; set; }
        public bool AutoUncheckAccount { get; set; }
        public Size SavedSize { get; set; }
        public List<Account> Accounts { get; set; }
        public List<Group> Groups { get; set; }

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        ///  Créer un fichier configuration s'il n'existe pas, sinon le lit
        /// </summary>
        public void GenConfig()
        {
            UUID = Guid.NewGuid();
            AL_Path = @"C:\Users\" + Environment.GetEnvironmentVariable("USERNAME") + @"\AppData\Local\Programs\zaap\Ankama Launcher.exe";
            StayLog = false;
            RetroMode = false;
            DiscordEnabled = false;
            AutoOrganizer = false;
            AutoUncheckAccount = false;
            SavedSize = new Size(250, 200);
            Accounts = new List<Account>();
            Groups = new List<Group>();

            try
            {
                if (!File.Exists("config.sser"))
                {
                    File.Create("config.sser").Close();
                    App.logstream.Log("config created");
                    if (File.Exists("config.ser"))
                        UpgradeConfig();
                    SaveConfig();
                }
                else
                    LoadConfig();
            }
            catch (Exception e) { App.logstream.Error(e); }
        }

        private void UpgradeConfig()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save into config.ser and load from config.ser
        /// </summary>
        public void UpdateConfig()
        {
            SaveConfig();
            LoadConfig();
        }

        /// <summary>
        /// Save into config.ser
        /// </summary>
        public void SaveConfig()
        {
            using (var stream = File.Open("config.ser", FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                App.logstream.Log("config saved");
            }
        }

        /// <summary>
        /// Load from config.ser
        /// </summary>
        public void LoadConfig()
        {
            using (var stream = File.Open("config.ser", FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                var config = (Config)formatter.Deserialize(stream);

                foreach (var field in config.GetType().GetProperties())
                {
                    GetType().GetProperty(field.Name).SetValue(this, field.GetValue(config));
                    App.logstream.Log(field.Name + " loaded");
                    if (GetType().GetProperty(field.Name).GetValue(this) == null) // If the field is null we initialize it
                        GetType().GetProperty(field.Name).SetValue(this, Activator.CreateInstance(GetType().GetProperty(field.Name).PropertyType));
                }
                if (UUID == new Guid("00000000-0000-0000-0000-000000000000")) // If the UUID is empty
                    UUID = new Guid();
            }
        }

        #endregion Public Methods
    }
}