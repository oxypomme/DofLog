using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DofLog
{
    public class Config
    {
        #region Public Fields

        public string AL_Path { get; set; }
        public bool StayLog { get; set; }
        public bool UpperAccountsName { get; set; }
        public bool RetroMode { get; set; }
        public List<Account> Accounts { get; set; }

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        ///  Créer un fichier configuration s'il n'existe pas, sinon le lit
        /// </summary>
        public void GenConfig()
        {
            if (!File.Exists("config.json"))
            {
                File.Create("config.json").Close();

                AL_Path = @"C:\Users\" + Environment.GetEnvironmentVariable("USERNAME") + @"\AppData\Local\Programs\zaap\Ankama Launcher.exe";
                StayLog = false;
                UpperAccountsName = true;
                RetroMode = false;
                Accounts = new List<Account>();

                UpdateConfigJSON();
            }
            else
            {
                Config config;
                using (StreamReader file = new StreamReader("config.json"))
                {
                    config = JsonConvert.DeserializeObject<Config>(file.ReadToEnd());
                    file.Close();
                }

                AL_Path = config.AL_Path;
                StayLog = config.StayLog;
                UpperAccountsName = config.UpperAccountsName;
                RetroMode = config.RetroMode;
                Accounts = config.Accounts;
            }
        }

        public void UpdateConfigJSON()
        {
            using (StreamWriter file = new StreamWriter("config.json"))
            {
                file.Write(JsonConvert.SerializeObject(this));
                file.Close();
            }
        }

        #endregion Public Methods
    }
}