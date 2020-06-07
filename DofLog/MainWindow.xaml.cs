using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

            Reload_lb_accounts();
        }

        private void Reload_lb_accounts()
        {
            lb_accounts.Items.Clear();

            var account_cm = new ContextMenu();

            var account_cm_edit = new MenuItem
            {
                Header = "Editer"
            };
            account_cm_edit.Click += Account_cm_edit_Click; ;

            var account_cm_del = new MenuItem
            {
                Header = "Supprimer"
            };
            account_cm_del.Click += Account_cm_del_Click;

            account_cm.Items.Add(account_cm_edit);
            account_cm.Items.Add(account_cm_del);

            foreach (var account in App.config.Accounts)
            {
                lb_accounts.Items.Add(new CheckBox
                {
                    Content = account,
                    ContextMenu = account_cm
                });
            }
        }

        private void Account_cm_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var senderAccount = (Account)((CheckBox)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget).Content;
                var newAccountDialog = new NewAccount(senderAccount);
                newAccountDialog.ShowDialog();
                if (!senderAccount.Equals(newAccountDialog.createdAccount) && newAccountDialog.createdAccount != null)
                {
                    var index = App.config.Accounts.IndexOf(senderAccount);
                    App.config.Accounts[index] = new Account(newAccountDialog.createdAccount);
                    App.config.UpdateConfig();
                    Reload_lb_accounts();
                }
            }
            catch (Exception ex) { App.logstream.Error(ex); }
        }

        private void Account_cm_del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.config.Accounts.Remove(
                    (Account)((CheckBox)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget).Content
                );
                App.config.UpdateConfig();
                Reload_lb_accounts();
                App.logstream.Log("Account removed");
            }
            catch (Exception ex) { App.logstream.Error(ex); }
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        { //TODO: ordre
            try
            {
                var checkedAccounts = new List<Account>();
                foreach (CheckBox acc in lb_accounts.Items)
                {
                    if (acc.IsChecked.HasValue && acc.IsChecked.Value)
                        checkedAccounts.Add((Account)acc.Content);
                }
                App.logger.LogAccounts(checkedAccounts);
            }
            catch (Exception ex) { App.logstream.Error(ex); }
        }

        private void lb_accounts_cm_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newAccountDialog = new NewAccount();
                newAccountDialog.ShowDialog();
                if (newAccountDialog.createdAccount != null)
                {
                    App.config.Accounts.Add(newAccountDialog.createdAccount);
                    App.config.UpdateConfig();
                    Reload_lb_accounts();
                }
            }
            catch (Exception ex) { App.logstream.Error(ex); }
        }
    }
}