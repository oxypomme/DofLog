using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DofLog
{
    /// <summary>
    /// Logique d'interaction pour SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();

            tb_al.Text = App.config.AL_Path;
            cb_staylog.IsChecked = App.config.StayLog;
            cb_retro.IsChecked = App.config.RetroMode;
            lbl_version.Content = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void cb_staylog_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void cb_retro_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void btn_al_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}