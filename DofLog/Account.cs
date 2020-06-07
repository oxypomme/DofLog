using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofLog
{
    [Serializable]
    public class Account
    {
        // TODO : Security
        #region Public Fields

        public string nickname;
        public string username;
        public string password;

        #endregion Public Fields

        #region Constructor

        public Account(string nickname, string username, string password)
        {
            this.nickname = nickname;
            this.username = username;
            this.password = password;
        }

        #endregion Constructor

        #region Public Methods

        public override string ToString()
        {
            return nickname;
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