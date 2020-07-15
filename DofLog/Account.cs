using System;

namespace DofLog
{
    [Serializable()]
    public class Account
    {
        // TODO #1 : Security

        private string username { get; set; }
        private string password { get; set; }

        #region Public Fields

        public string nickname;
        public string UsernameCipher { get => StringCipher.Decrypt(username); private set => username = StringCipher.Encrypt(value); }
        public string PasswordCipher { get => StringCipher.Decrypt(password); private set => password = StringCipher.Encrypt(value); }

        #endregion Public Fields

        #region Constructors

        public Account(string nickname, string username, string password)
        {
            this.nickname = nickname;
            UsernameCipher = username;
            PasswordCipher = password;
        }

        public Account(Account acc)
        {
            nickname = acc.nickname;
            UsernameCipher = acc.UsernameCipher;
            PasswordCipher = acc.PasswordCipher;
        }

        #endregion Constructors

        #region Public Methods

        public void CleanLogs()
        {
            UsernameCipher = username;
            PasswordCipher = password;
        }

        public override string ToString() => nickname;

        public override bool Equals(object obj)
        {
            if (obj is Account)
            {
                var acc = (Account)obj;
                if (acc.nickname == nickname && acc.username == username && acc.password == password)
                    return true;
            }
            return false;
        }

        public static bool Equals(Account acc1, Account acc2)
        {
            if (acc2.nickname == acc1.nickname && acc2.username == acc1.username && acc2.password == acc1.password)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Public Methods
    }
}