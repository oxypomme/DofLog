using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofLog
{
    public class Group : IList<Account>
    {
        public string name;
        public List<Account> accounts;

        #region Constructors

        public Group(string name, List<Account> accounts = null)
        {
            if (accounts == null)
                accounts = new List<Account>();
            this.name = name;
            this.accounts = accounts;
        }

        public Group(Group grp)
        {
            name = grp.name;
            accounts = grp.accounts;
        }

        #endregion Constructors

        #region Public Methods

        public override string ToString() => name;

        public Account this[int index] { get => accounts[index]; set => throw new NotImplementedException(); }

        public int Count => accounts.Count;

        public bool IsReadOnly => true;

        public void Add(Account item) => accounts.Add(item);

        public void Clear() => accounts.Clear();

        public bool Contains(Account item) => accounts.Contains(item);

        public void CopyTo(Account[] array, int arrayIndex) => accounts.CopyTo(array, arrayIndex);

        public IEnumerator<Account> GetEnumerator() => accounts.GetEnumerator();

        public int IndexOf(Account item) => accounts.IndexOf(item);

        public void Insert(int index, Account item) => accounts.Insert(index, item);

        public bool Remove(Account item) => accounts.Remove(item);

        public void RemoveAt(int index) => accounts.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => accounts.GetEnumerator();

        #endregion Public Methods
    }
}