﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DofLog
{
    [Serializable()]
    public class Config
    {
        #region Public Fields

        public string AL_Path { get; set; }
        public bool StayLog { get; set; }
        public bool RetroMode { get; set; }
        public List<Account> Accounts { get; set; }

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        ///  Créer un fichier configuration s'il n'existe pas, sinon le lit
        /// </summary>
        public void GenConfig()
        {
            AL_Path = @"C:\Users\" + Environment.GetEnvironmentVariable("USERNAME") + @"\AppData\Local\Programs\zaap\Ankama Launcher.exe";
            StayLog = false;
            RetroMode = false;
            Accounts = new List<Account>();

            try
            {
                if (!File.Exists("config.ser"))
                {
                    File.Create("config.ser").Close();
                    App.logstream.Log("config created");
                    SaveConfig();
                }
                else
                    LoadConfig();
            }
            catch (Exception e) { App.logstream.Error(e); }
        }

        public void UpdateConfig()
        {
            SaveConfig();
            LoadConfig();
        }

        public void SaveConfig()
        {
            using (var stream = File.Open("config.ser", FileMode.Create))
            {
                // Serialize config
                // TODO BUG: Accounts not saved

                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                App.logstream.Log("config saved");
            }
        }

        public void LoadConfig()
        {
            using (var stream = File.Open("config.ser", FileMode.Open))
            {
                // Deserialize config
                var formatter = new BinaryFormatter();
                var config = (Config)formatter.Deserialize(stream);

                foreach (var field in config.GetType().GetProperties())
                {
                    GetType().GetProperty(field.Name).SetValue(this, field.GetValue(config));
                    App.logstream.Log(field.Name + " loaded");
                }
            }
        }

        #endregion Public Methods
    }
}