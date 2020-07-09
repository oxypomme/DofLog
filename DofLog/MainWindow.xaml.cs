using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
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
        //TODO: Raccourcis claviers

        #region Private Fields

        private Forms.NotifyIcon notify;

        #endregion Private Fields

        public MainWindow()
        {
            InitializeComponent();
            notify = new Forms.NotifyIcon();

            // Creating check box for each account saved
            foreach (var account in App.config.Accounts)
                lb_accounts.Items.Add(CreateAccountCheckBox(account));

            // Creating the context menu of the notify
            var cmNotify = new Forms.ContextMenu();
            {
                var item = new Forms.MenuItem();

                item.Text = "Doflog - v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                item.Enabled = false;
                cmNotify.MenuItems.Add(item);

                item = new Forms.MenuItem();
                item.Text = "&Afficher";
                item.Click += NotifyMenu_ShowClick;
                cmNotify.MenuItems.Add(item);

                item = new Forms.MenuItem();
                item.Text = "&Quitter";
                item.Click += NotifyMenu_QuitClick;
                cmNotify.MenuItems.Add(item);
            }

            // Setting up the notify
            notify.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            notify.ContextMenu = cmNotify;
            notify.Click += NotifyMenu_ShowClick;
            notify.Visible = true;

            btn_discord.IsChecked = App.config.DiscordEnabled;

            Width = App.config.SavedSize.Width;
            Height = App.config.SavedSize.Height;

            ReloadTheme();
        }

        #region WPF

        public void ReloadTheme()
        {
            App.ReloadTheme();
            Background = (System.Windows.Media.SolidColorBrush)FindResource("BackgroundColor");
            UpdateDefaultStyle();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            App.config.SavedSize = new System.Drawing.Size((int)Width, (int)Height);
            App.config.UpdateConfig();
        }

        #endregion WPF

        #region Account List

        private ContextMenu CreateAccountContextMenu()
        {
            var item_cm = new ContextMenu();
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
                    item_cm.Items.Add(item);
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
                    item_cm.Items.Add(item);
                }
            }
            return item_cm;
        }

        private CheckBox CreateAccountCheckBox(Account account)
        {
            var item = new CheckBox
            {
                Content = account,
                ContextMenu = CreateAccountContextMenu()
            };
            item.Checked += Account_Checked;
            item.Unchecked += Account_Unchecked;
            return item;
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

        #endregion Account List

        #region Account Buttons

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
                    lb_accounts.Items.Add(CreateAccountCheckBox(newAccountDialog.createdAccount));
                    App.config.UpdateConfig();
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
                CheckBox realSender;
                if (sender is MenuItem)
                    realSender = (CheckBox)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget;
                else if (sender is Button && lb_accounts.SelectedItems.Count > 0)
                    realSender = (CheckBox)lb_accounts.SelectedItem;
                else
                    throw new NullReferenceException();
                var newAccountDialog = new NewAccountDialog((Account)realSender.Content);
                newAccountDialog.Owner = this;
                newAccountDialog.ShowDialog();
                if (!realSender.Content.Equals(newAccountDialog.createdAccount) && newAccountDialog.createdAccount != null)
                {
                    var index = App.config.Accounts.IndexOf((Account)realSender.Content);
                    App.config.Accounts[index] = new Account(newAccountDialog.createdAccount);
                    lb_accounts.Items[index] = CreateAccountCheckBox(newAccountDialog.createdAccount);
                    App.config.UpdateConfig();
                }
                lb_accounts.SelectedItem = realSender;
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
                CheckBox realSender;
                if (sender is MenuItem)
                    realSender = (CheckBox)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget;
                else if (sender is Button && lb_accounts.SelectedItems.Count > 0)
                    realSender = (CheckBox)lb_accounts.SelectedItem;
                else
                    throw new NullReferenceException();
                App.config.Accounts.Remove((Account)realSender.Content);
                lb_accounts.Items.Remove(realSender);
                App.config.UpdateConfig();
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

        private void ClearSlectedAccounts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (CheckBox item in lb_accounts.Items)
                    if (item.IsChecked.Value)
                        item.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Une erreur inattendue est survenue...", MessageBoxButton.OK, MessageBoxImage.Error);
                App.logstream.Error(ex);
            }
        }

        private void UpAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox realSender;
                if (sender is MenuItem)
                    realSender = (CheckBox)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget;
                else if (sender is Button && lb_accounts.SelectedItems.Count > 0)
                    realSender = (CheckBox)lb_accounts.SelectedItem;
                else
                    throw new NullReferenceException();

                int index = App.config.Accounts.IndexOf((Account)realSender.Content);
                if (index > 0)
                {
                    App.config.Accounts.Remove((Account)realSender.Content);
                    App.config.Accounts.Insert(index - 1, (Account)realSender.Content);
                    lb_accounts.Items.Remove(realSender);
                    lb_accounts.Items.Insert(index - 1, realSender);
                    App.config.UpdateConfig();
                }
                lb_accounts.SelectedItem = realSender;
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

        private void DownAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox realSender;
                if (sender is MenuItem)
                    realSender = (CheckBox)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget;
                else if (sender is Button && lb_accounts.SelectedItems.Count > 0)
                    realSender = (CheckBox)lb_accounts.SelectedItem;
                else
                    throw new NullReferenceException();

                int index = App.config.Accounts.IndexOf((Account)realSender.Content);
                if (index < App.config.Accounts.Count)
                {
                    App.config.Accounts.Remove((Account)realSender.Content);
                    App.config.Accounts.Insert(index + 1, (Account)realSender.Content);
                    lb_accounts.Items.Remove(realSender);
                    lb_accounts.Items.Insert(index + 1, realSender);
                    App.config.UpdateConfig();
                }
                lb_accounts.SelectedItem = realSender;
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

        #endregion Account Buttons

        #region Notify

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
            App.StopRPC();
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

        #endregion Notify

        #region Other buttons

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Forms.Cursor.Current = Forms.Cursors.WaitCursor;

                var cancelLogTaskSource = new CancellationTokenSource();
                var ct = cancelLogTaskSource.Token;

                var logTask = Task.Run(() => { Logger.LogAccounts(ct); }, cancelLogTaskSource.Token);
                if (!logTask.Wait(Logger.PAUSE * 400 * Logger.accounts.Count))
                {
                    cancelLogTaskSource.Cancel();
                    throw new TimeoutException();
                }
                logTask.Dispose();

                Forms.Cursor.Current = Forms.Cursors.Default;

                var sb = new System.Text.StringBuilder();
                foreach (var acc in Logger.accounts)
                {
                    sb.Append(acc);
                    if (Logger.accounts.IndexOf(acc) + 2 == Logger.accounts.Count)
                        sb.Append(" et ");
                    else if (Logger.accounts.IndexOf(acc) + 1 != Logger.accounts.Count)
                        sb.Append(", ");
                }
                if (Logger.accounts.Count > 1)
                    sb.Append(" sont");
                else
                    sb.Append(" est");
                notify.ShowBalloonTip(5000, "Tout les comptes sont connectés", sb.ToString() + " connectés !", Forms.ToolTipIcon.Info);
                Task.Run(() =>
                {
                    Thread.Sleep(5000);
                    Logger.state = Logger.LoggerState.IDLE;
                });
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
            App.StartRPC();
        }

        private void btn_discord_Unchecked(object sender, RoutedEventArgs e)
        {
            App.StopRPC();
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

        #endregion Other buttons
    }
}