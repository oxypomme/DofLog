using System;

namespace DofLog
{
    [Serializable()]
    public class Account
    {
        // TODO : Security

        #region Public Fields

        public string nickname;
        public string username;
        public string password;

        #endregion Public Fields

        #region Constructors

        public Account(string nickname, string username, string password)
        {
            this.nickname = nickname;
            this.username = username;
            this.password = password;
        }

        public Account(Account acc)
        {
            nickname = acc.nickname;
            username = acc.username;
            password = acc.password;
        }

        #endregion Constructors

        #region Public Methods

        public override string ToString()
        {
            return nickname;
        }

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

        #region Private Methods

        private string GetUsername(string rawUsername)
        {
            return rawUsername;
        }

        private string GetPassword(string rawPassword)
        {
            return rawPassword;
        }

        #endregion Private Methods
    }
}