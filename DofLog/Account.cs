using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofLog
{
    public class Account
    {
        #region Public Fields

        public string nickname;
        public string username;
        public string password;

        #endregion Public Fields

        #region Constructor

        public Account(string rawAccount)
        {
            nickname = rawAccount.Split('/')[0];
            username = GetUsername(rawAccount.Split('/')[1]);
            password = GetPassword(rawAccount.Split('/')[2]);
        }

        #endregion Constructor

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