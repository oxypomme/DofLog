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

            lb_accounts.Items.Clear(); //DEBUG purposes

            var Accounts = new List<Account>();
            Accounts.Add(new Account("Account 1", "fauxcompte", "biententé"));
            Accounts.Add(new Account("Account 2", "fauxcompte", "biententé"));
            Accounts.Add(new Account("Account 3", "fauxcompte", "biententé"));
            Accounts.Add(new Account("Account 4", "fauxcompte", "biententé"));

            var account_cm = new ContextMenu();
            account_cm.Items.Add(new MenuItem
            {
                Header = "Supprimer"
            });

            foreach(var account in Accounts)
            {
                lb_accounts.Items.Add(new CheckBox
                {
                    Content = account,
                    ContextMenu = account_cm
                });
            }
        }
    }
}
