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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DofLog
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            App.config.GenConfig();

            var account_cm = new ContextMenu();
            account_cm.Items.Add(new MenuItem
            {
                Header = "Supprimer"
            });

            foreach(var account in App.config.Accounts)
            {
                lb_accounts.Items.Add(new CheckBox
                {
                    Content = account,
                    ContextMenu = account_cm
                });
            }
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            var checkedAccounts = new List<Account>();
            foreach (CheckBox acc in lb_accounts.Items)
            {
                if (acc.IsChecked.HasValue && acc.IsChecked.Value)
                    checkedAccounts.Add((Account) acc.Content);
            }
            App.logger.LogAccounts(checkedAccounts);
        }
    }
}
