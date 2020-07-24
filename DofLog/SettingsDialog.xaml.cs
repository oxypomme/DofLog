using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Forms = System.Windows.Forms;

namespace DofLog
{
    /// <summary>
    /// Logique d'interaction pour SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        #region Constructor

        public SettingsDialog()
        {
            InitializeComponent();

            foreach (var field in App.config.GetType().GetProperties())
            {
                try
                {
                    if (field.PropertyType == typeof(bool)) // if it's a bool, a checkbox is needed
                        ((CheckBox)FindName("cb_" + field.Name.ToLower())).IsChecked = (bool?)field.GetValue(App.config);
                    else if (field.PropertyType == typeof(string)) // if it's a string, a textbox is needed
                        ((TextBox)FindName("tb_" + field.Name.ToLower())).Text = (string)field.GetValue(App.config);
                }
                catch (NullReferenceException) { App.logstream.Warning("Config Window missing an item : " + field.Name); }
            }

            lbl_version.Content = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            ReloadTheme();
        }

        #endregion Constructor

        #region WPF

        private void ReloadTheme()
        {
            App.ReloadTheme();
            Background = (SolidColorBrush)FindResource("BackgroundColor");
            UpdateDefaultStyle();
        }

        #endregion WPF

        #region Checkbox events

        private void cb_staylog_Checked(object sender, RoutedEventArgs e)
        {
            App.config.StayLog = true;
            App.config.UpdateConfig();
        }

        private void cb_staylog_Unchecked(object sender, RoutedEventArgs e)
        {
            App.config.StayLog = false;
            App.config.UpdateConfig();
        }

        private void cb_retro_Unchecked(object sender, RoutedEventArgs e)
        {
            App.config.RetroMode = false;
            App.config.UpdateConfig();
            ReloadTheme();
        }

        private void cb_retro_Checked(object sender, RoutedEventArgs e)
        {
            App.config.RetroMode = true;
            App.config.UpdateConfig();
            ReloadTheme();
        }

        private void cb_organizer_Checked(object sender, RoutedEventArgs e)
        {
            App.config.AutoOrganizer = true;
            App.config.UpdateConfig();
        }

        private void cb_organizer_Unchecked(object sender, RoutedEventArgs e)
        {
            App.config.AutoOrganizer = false;
            App.config.UpdateConfig();
        }

        private void cb_uncheck_Checked(object sender, RoutedEventArgs e)
        {
            App.config.AutoUncheckAccount = true;
            App.config.UpdateConfig();
        }

        private void cb_uncheck_Unchecked(object sender, RoutedEventArgs e)
        {
            App.config.AutoUncheckAccount = false;
            App.config.UpdateConfig();
        }

        #endregion Checkbox events

        #region Buttons events

        private void btn_al_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = App.config.AL_Path;
            openFileDialog.Filter = "Executables (*.exe)|*.exe|Tout les fichiers (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == Forms.DialogResult.OK)
            {
                App.config.AL_Path = openFileDialog.FileName;
                tb_al_path.Text = openFileDialog.FileName;
                App.config.UpdateConfig();
            }
        }

        #endregion Buttons events
    }
}