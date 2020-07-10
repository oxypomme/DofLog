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
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class NewGroupDialog : Window
    {
        #region Internal Fields

        internal Group createdGroup { get; set; }

        #endregion Internal Fields

        public NewGroupDialog()
        {
            InitializeComponent();
            InitShortcuts();

            foreach (var acc in App.config.Accounts)
                lb_accounts.Items.Add(new CheckBox
                {
                    Content = acc
                });

            tb_name.Focus();
        }

        public NewGroupDialog(Group grp)
        {
            InitializeComponent();
            InitShortcuts();

            foreach (var acc in App.config.Accounts)
                lb_accounts.Items.Add(new CheckBox
                {
                    Content = acc
                });

            tb_name.Text = grp.name;

            foreach (CheckBox account in lb_accounts.Items)
            {
                if (grp.Contains(account.Content))
                    account.IsChecked = true;
            }
        }

        #region Buttons events

        private void InitShortcuts()
        {
            // inspired by https://stackoverflow.com/a/33450624
            RoutedCommand keyShortcut = new RoutedCommand();

            /* OK SHORTCUT (Enter) */
            keyShortcut.InputGestures.Add(new KeyGesture(Key.Enter));
            CommandBindings.Add(new CommandBinding(keyShortcut, btn_ok_Click));

            /* CANCEL SHORTCUT (Escape) */
            keyShortcut = new RoutedCommand();
            keyShortcut.InputGestures.Add(new KeyGesture(Key.Escape));
            CommandBindings.Add(new CommandBinding(keyShortcut, btn_cancel_Click));
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            var accounts = new List<Account>();
            foreach (CheckBox acc in lb_accounts.Items)
            {
                if (acc.IsChecked.Value)
                    accounts.Add((Account)acc.Content);
            }
            createdGroup = new Group(tb_name.Text, accounts);
            Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e) => Close();

        #endregion Buttons events
    }
}