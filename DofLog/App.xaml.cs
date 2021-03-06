﻿using DiscordRPC;
using DofLog.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DofLog
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static Config config { get; private set; }
        internal static Logger Logger { get; private set; }
        internal static DateTime startTime;

        public App()
        {
            try
            {
                if ((Guid)Settings.Default["UUID"] == new Guid("00000000-0000-0000-0000-000000000000")) // If the UUID is empty
                {
                    Settings.Default["UUID"] = Guid.NewGuid();
                    Settings.Default.Save();
                }

                logstream = new LogStream(Path.Combine(Environment.CurrentDirectory, "logs.log"));

                config = new Config();
                config.GenConfig();

                Logger = new Logger();

                startTime = DateTime.Now;
                StartRPC();

                Updater();
            }
            catch (Exception e) { logstream.Error(e); }
        }

        #region Utils

        public static LogStream logstream { get; set; }

        /// <summary>
        /// Check if a color is nearly the same of another
        /// </summary>
        /// <param name="colorBase">The color that you searched</param>
        /// <param name="colorFound">The color that you find</param>
        /// <param name="tolerance">The tolerance (inclusive), by default 10</param>
        /// <returns>`true` if the color is nearly the same, `false` if doesn't</returns>
        public static bool IsAroundColor(Color colorBase, Color colorFound, int tolerance = 10)
        {
            //logstream.Log(colorFound, "DEBUG");
            if (colorBase.R - tolerance <= colorFound.R && colorFound.R <= colorBase.R + tolerance)
                if (colorBase.G - tolerance <= colorFound.G && colorFound.G <= colorBase.G + tolerance)
                    if (colorBase.B - tolerance <= colorFound.B && colorFound.B <= colorBase.B + tolerance)
                        return true;
            return false;
        }

        /// <summary>
        /// Round a float and converts it to an int
        /// </summary>
        /// <param name="value">The float</param>
        /// <returns>An int</returns>
        public static int RoundFloat(float value)
        {
            return Convert.ToInt32(Math.Round(value));
        }

        /// <summary>
        /// Reload the WPF theme
        /// </summary>
        public static void ReloadTheme()
        {
            Current.Resources.MergedDictionaries.Clear();

            var ressource = new ResourceDictionary();
            if (config.RetroMode)
            {
                ressource.Source = new Uri("pack://application:,,,/DofLog;component/Themes/DofusRetro/DofusRetro.xaml", UriKind.RelativeOrAbsolute);
                Current.Resources.MergedDictionaries.Add(ressource);
            }
            else
            {
                ressource.Source = new Uri("pack://application:,,,/DofLog;component/Themes/DofusDark/DofusDark.xaml", UriKind.RelativeOrAbsolute);
                Current.Resources.MergedDictionaries.Add(ressource);
            }

            ressource = new ResourceDictionary();

            ressource.Source = new Uri("pack://application:,,,/DofLog;component/Themes/Dofus.xaml", UriKind.RelativeOrAbsolute);
            Current.Resources.MergedDictionaries.Add(ressource);
        }

        /// <summary>
        /// Check if there's a new update on GitHub
        /// </summary>
        public async static void Updater()
        {
            try
            {
                if (File.Exists("DofLog_setup.exe"))
                    File.Delete("DofLog_setup.exe");
                /* Credits to WildGoat07 : https://github.com/WildGoat07 */
                var github = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("DofLog"));
                var lastRelease = await github.Repository.Release.GetLatest(270258000);
                var current = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                logstream.Log($"Versions : Current = {current}, Latest = {lastRelease.TagName}");
                if (new Version(lastRelease.TagName) > current)
                {
                    var result = MessageBox.Show("Une nouvelle version est disponible. Voulez-vous la télécharger ?", "Updater", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        WebClient webClient = new WebClient();
                        logstream.Log("Downloading Update");
                        webClient.DownloadFile(new Uri("https://github.com/oxypomme/DofLog/releases/latest/download/DofLog_setup.exe"), "DofLog_setup.exe");
                        logstream.Log("Updated downloaded, extracting it");
                        Process.Start(new ProcessStartInfo(Path.Combine(Environment.CurrentDirectory, "DofLog_setup.exe")));
                        Environment.Exit(0);
                    }
                }
                else if (new Version(lastRelease.TagName) < current)
                {
                    var result = MessageBox.Show("Cette version de DofLog est expérimentale : de nombreux bugs peuvent survenir et les nouvelles fonctionnalités peuvent ne pas être prêtes à l'utilisation. Voulez-vous continuer ?", "Updater", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                    if (result == MessageBoxResult.No)
                    {
                        StopRPC();
                        logstream.Close();
                        Environment.Exit(1);
                    }
                }
            }
            catch (Exception e) { logstream.Error(e); }
            finally
            {
                //Current.MainWindow.Dispatcher.Invoke(new Action(() => Current.MainWindow.Show()));
            }
        }

        #endregion Utils

        #region Organizer

        /// <summary>
        /// Launch the Organizer module
        /// </summary>
        /// <returns>The process of the Organizer module</returns>
        public static Process LaunchOrganizer()
        {
            if (!File.Exists("Modules/Organizer.exe"))
                DownloadOrganizer();
            foreach (var proc in Process.GetProcessesByName("Organizer"))
                proc.Kill();
            return Process.Start(new ProcessStartInfo(Path.Combine(Environment.CurrentDirectory, "Modules/Organizer.exe")));
        }

        /// <summary>
        /// Download the organizer module
        /// </summary>
        public static void DownloadOrganizer()
        {
            WebClient webClient = new WebClient();
            logstream.Log("Downloading Organizer");
            webClient.DownloadFile(new Uri("http://update.naio.fr/v2/Organizer/1.4/Organizer.zip"), "Organizer.zip");
            logstream.Log("Organizer downloaded, extracting it");
            System.IO.Compression.ZipFile.ExtractToDirectory("Organizer.zip", Environment.CurrentDirectory);
            logstream.Log("Organizer extracted");
            File.Delete("Organizer.zip");
        }

        #endregion Organizer

        #region Discord

        private static CancellationTokenSource rpcUpdaterToken;

        public static void StartRPC()
        {
            if (config.DiscordEnabled)
            {
                var discordClient = new DiscordRpcClient("623896785605361664");
                try
                {
                    discordClient.Initialize();

                    rpcUpdaterToken = new CancellationTokenSource();
                    var ct = rpcUpdaterToken.Token;
                    Task.Run(() =>
                       {
                           while (config.DiscordEnabled)
                           {
                               if (ct.IsCancellationRequested)
                                   break;

                               var dofs = Process.GetProcessesByName("dofus").ToList();
                               if (Logger.State == Logger.LoggerState.CONNECTING)
                                   UpdateRPC("Se connecte...", "Comptes connectés :", dofs.Count, Logger.Accounts.Count);
                               else if (dofs.Count() > 0)
                               {
                                   var sb = new System.Text.StringBuilder();
                                   foreach (var process in dofs)
                                   {
                                       if (!process.MainWindowTitle.StartsWith("Dofus"))
                                       {
                                           sb.Append(process.MainWindowTitle.Split('-')[0].Trim());

                                           if (dofs.IndexOf(process) + 2 == dofs.Count)
                                               sb.Append(" et ");
                                           else if (dofs.IndexOf(process) + 1 != dofs.Count)
                                               sb.Append(", ");
                                       }
                                   }
                                   UpdateRPC("Connecté avec :", sb.ToString(), dofs.Count);
                               }
                               else
                                   UpdateRPC("Se prépare...");
                               Thread.Sleep(1000);
                           }
                           discordClient.ClearPresence();
                           discordClient.Dispose();
                       }, rpcUpdaterToken.Token);
                }
                catch (Exception e)
                {
                    logstream.Log(e);
                }

                void UpdateRPC(string message, string state = "", int accountCount = 0, int maxCount = 8)
                {
                    discordClient.SetPresence(new RichPresence()
                    {
                        Details = message,
                        State = state,
                        Party = new Party()
                        {
                            ID = Guid.NewGuid().ToString(),
                            Size = accountCount,
                            Max = maxCount
                        },
                        Timestamps = new Timestamps()
                        {
                            Start = (Process.GetProcessesByName("dofus").ToList().Count > 0 && Logger.State != Logger.LoggerState.CONNECTING ? startTime.ToUniversalTime() : DateTime.Now.AddSeconds(1).ToUniversalTime())
                        },
                        Assets = new Assets()
                        {
                            LargeImageKey = "header",
                            SmallImageKey = (!config.RetroMode ? "dofuslogo" : "dofusrlogo"),
                            SmallImageText = (!config.RetroMode ? "Dofus 2" : "Dofus Retro")
                        }
                    });
                }
            }
        }

        public static void StopRPC()
        {
            if (config.DiscordEnabled)
            {
                rpcUpdaterToken.Cancel();
                rpcUpdaterToken.Dispose();
            }
        }

        #endregion Discord
    }
}