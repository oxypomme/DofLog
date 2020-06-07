using System.Windows;

namespace DofLog
{
    /// <summary>
    /// Logique d'interaction pour NewAccount.xaml
    /// </summary>
    public partial class NewAccount : Window
    {
        internal Account createdAccount;

        public NewAccount()
        {
            InitializeComponent();
        }

        public NewAccount(Account acc)
        {
            InitializeComponent();

            tb_nickname.Text = acc.nickname;
            tb_username.Text = acc.username;
            tb_password.Password = acc.password;
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Bind enter
            createdAccount = new Account(tb_nickname.Text, tb_username.Text, tb_password.Password);
            Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}