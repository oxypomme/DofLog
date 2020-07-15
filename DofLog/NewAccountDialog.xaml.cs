using System.Windows;
using System.Windows.Input;

namespace DofLog
{
    /// <summary>
    /// Logique d'interaction pour NewAccount.xaml
    /// </summary>
    ///
    public partial class NewAccountDialog : Window
    {
        #region Internal Fields

        internal Account createdAccount { get; set; }

        #endregion Internal Fields

        #region Constructor

        public NewAccountDialog()
        {
            InitializeComponent();
            InitShortcuts();

            tb_nickname.Focus();
        }

        public NewAccountDialog(Account acc)
        {
            InitializeComponent();
            InitShortcuts();

            tb_nickname.Text = acc.nickname;
            tb_username.Text = acc.UsernameCipher;
            tb_password.Password = acc.PasswordCipher;
        }

        #endregion Constructor

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
            createdAccount = new Account(tb_nickname.Text, tb_username.Text, tb_password.Password);
            Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e) => Close();

        #endregion Buttons events
    }
}