using System.Windows;

namespace DofLog
{
    /// <summary>
    /// Logique d'interaction pour NewAccount.xaml
    /// </summary>
    ///
    public partial class NewAccountDialog : Window
    {
        //TODO #4 : Raccourcis claviers

        #region Internal Fields

        internal Account createdAccount { get; set; }

        #endregion Internal Fields

        #region Constructor

        public NewAccountDialog()
        {
            InitializeComponent();
        }

        public NewAccountDialog(Account acc)
        {
            InitializeComponent();

            tb_nickname.Text = acc.nickname;
            tb_username.Text = acc.username;
            tb_password.Password = acc.password;
        }

        #endregion Constructor

        #region Buttons events

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            createdAccount = new Account(tb_nickname.Text, tb_username.Text, tb_password.Password);
            Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e) => Close();

        #endregion Buttons events
    }
}
