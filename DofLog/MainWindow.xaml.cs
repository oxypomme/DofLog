using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace DofLog
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Forms.NotifyIcon notify;

        public MainWindow()
        {
            InitializeComponent();
            notify = new Forms.NotifyIcon();

            Reload_lb_accounts();

            // Creating the context menu of the notify
            var cmNotify = new Forms.ContextMenu();
            {
                var item = new Forms.MenuItem();

                item.Text = "Doflog - v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                item.Enabled = false;
                cmNotify.MenuItems.Add(item);

                item = new Forms.MenuItem();
                item.Text = "&Show";
                item.Click += NotifyMenu_ShowClick;
                cmNotify.MenuItems.Add(item);

                item = new Forms.MenuItem();
                item.Text = "&Quit";
                item.Click += NotifyMenu_QuitClick;
                cmNotify.MenuItems.Add(item);
            }

            // Setting up the notify
            notify.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            notify.ContextMenu = cmNotify;
            notify.Click += NotifyMenu_ShowClick;
            notify.Visible = true;

            btn_discord.IsChecked = App.config.DiscordEnabled;

            ReloadTheme();
        }

        public void ReloadTheme()
        {
            App.ReloadTheme();
            Background = (System.Windows.Media.SolidColorBrush)FindResource("BackgroundColor");
            UpdateDefaultStyle();
        }

        private void Reload_lb_accounts()
        {
            lb_accounts.Items.Clear();

            // Creating the context menu of each check box
            var account_cm = new ContextMenu();
            {
                {
                    var item = new MenuItem()
                    {
                        Header = "Éditer",
                        Icon = new Image()
                        {
                            Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("img/edit.png", UriKind.Relative))
                        }
                    };
                    item.Click += EditAccount_Click;
                    account_cm.Items.Add(item);
                }
                {
                    var item = new MenuItem()
                    {
                        Header = "Supprimer",
                        Icon = new Image()
                        {
                            Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("img/remove.png", UriKind.Relative))
                        }
                    };
                    item.Click += DeleteAccount_Click;
                    account_cm.Items.Add(item);
                }
            }

            // Creating check box for each account saved
            foreach (var account in App.config.Accounts)
            {
                var item = new CheckBox
                {
                    Content = account,
                    ContextMenu = account_cm
                };
                item.Checked += Account_Checked;
                item.Unchecked += Account_Unchecked;
                lb_accounts.Items.Add(item);
            }
        }

        private void Account_Checked(object sender, RoutedEventArgs e)
        {
            try { Logger.accounts.Add((Account)((CheckBox)sender).Content); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur inattendue est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
        }

        private void Account_Unchecked(object sender, RoutedEventArgs e)
        {
            try { Logger.accounts.Remove((Account)((CheckBox)sender).Content); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur inattendue est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newAccountDialog = new NewAccountDialog();
                newAccountDialog.Owner = this;
                newAccountDialog.ShowDialog();
                if (newAccountDialog.createdAccount != null)
                {
                    App.config.Accounts.Add(newAccountDialog.createdAccount);
                    App.config.UpdateConfig();
                    Reload_lb_accounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur inattendue est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Account item;
                if (sender is MenuItem)
                    item = (Account)((CheckBox)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget).Content;
                else if (sender is Button && lb_accounts.SelectedItems.Count > 0)
                    item = (Account)((CheckBox)lb_accounts.SelectedItem).Content;
                else
                    throw new NullReferenceException();
                var newAccountDialog = new NewAccountDialog(item);
                newAccountDialog.Owner = this;
                newAccountDialog.ShowDialog();
                if (!item.Equals(newAccountDialog.createdAccount) && newAccountDialog.createdAccount != null)
                {
                    var index = App.config.Accounts.IndexOf(item);
                    App.config.Accounts[index] = new Account(newAccountDialog.createdAccount);
                    App.config.UpdateConfig();
                    Reload_lb_accounts();
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Veuillez sélectionner un compte à éditer.", "Une erreur est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur inattendue est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Account item;
                if (sender is MenuItem)
                    item = (Account)((CheckBox)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget).Content;
                else if (sender is Button)
                    item = (Account)((CheckBox)lb_accounts.SelectedItem).Content;
                else
                    throw new NullReferenceException();
                App.config.Accounts.Remove(item);
                App.config.UpdateConfig();
                Reload_lb_accounts();
                App.logstream.Log("Account removed");
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Veuillez sélectionner un compte à supprimer.", "Une erreur est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur inattendue est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
        }

        private void NotifyMenu_ShowClick(object sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
        }

        private void NotifyMenu_QuitClick(object sender, EventArgs e)
        {
            var Org_Process = System.Diagnostics.Process.GetProcessesByName("Organizer");
            foreach (var proc in Org_Process)
                proc.Kill();
            App.logstream.Close();
            Environment.Exit(1);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            notify.ShowBalloonTip(5000, "DofLog est maintenant réduit !", "Au moins il ne prend plus beaucoup de place...", Forms.ToolTipIcon.Info);
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
            e.Cancel = true;
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Forms.Cursor.Current = Forms.Cursors.WaitCursor;

                var logTask = System.Threading.Tasks.Task.Run(() => { Logger.LogAccounts(); });
                if (!logTask.Wait(Logger.PAUSE * 400 * Logger.accounts.Count))
                    throw new TimeoutException();

                Forms.Cursor.Current = Forms.Cursors.Default;

                var sb = new System.Text.StringBuilder();
                foreach (var acc in Logger.accounts)
                {
                    sb.Append(acc);
                    if (Logger.accounts.IndexOf(acc) + 2 == Logger.accounts.Count)
                        sb.Append(" et ");
                    else if (Logger.accounts.IndexOf(acc) + 1 != Logger.accounts.Count)
                        sb.Append(", ");
                    else if (Logger.accounts.IndexOf(acc) + 1 == Logger.accounts.Count)
                        if (Logger.accounts.Count > 1)
                            sb.Append(" sont");
                        else
                            sb.Append(" est");
                }
                notify.ShowBalloonTip(5000, "Tout les comptes sont connectés", sb.ToString() + " connectés !", Forms.ToolTipIcon.Info);
                if (App.config.AutoOrganizer)
                {
                    App.LaunchOrganizer();
                    //TODO? Logger.OrganizeAccounts()
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Veuillez sélectionner au moins un compte à connecter.", "Une erreur est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show("Veuillez redéfinir le chemin vers l'Ankama Launcher.\nVous pouvez aussi lancer l'Ankama Launcher avant de vous connecter.", "Une erreur est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show("La connexion à sûrement échouée, vérifier votre connexion internet et que vous n'avez pas bougé la souris pendant la connexion. Si ce problème persiste contacter l'équipe de développement.", "Une erreur est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur inattendue est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
            Forms.Cursor.Current = Forms.Cursors.Default;
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settingsDialog = new SettingsDialog();
                settingsDialog.Owner = this;
                settingsDialog.ShowDialog();
                ReloadTheme();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur inattendue est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
        }

        private void btn_discord_Checked(object sender, RoutedEventArgs e)
        {
            App.config.DiscordEnabled = true;
            App.config.UpdateConfig();
            //TODO: discord integration
        }

        private void btn_discord_Unchecked(object sender, RoutedEventArgs e)
        {
            App.config.DiscordEnabled = false;
            App.config.UpdateConfig();
        }

        private void btn_organizer_Click(object sender, RoutedEventArgs e)
        {
            try { App.LaunchOrganizer(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur inattendue est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
        }
    }
}