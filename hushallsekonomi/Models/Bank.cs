using System;
using System.Collections.Generic;
using System.Text;

namespace hushallsekonomi
{
    public class Bank
    {
        public string Name { get; set; }
        public List<BankAccount> BankAccounts { get; set; }

        /// <summary>
        /// Get all bank accounts for specific user
        /// </summary>
        /// <param name="user">specified user</param>
        /// <returns>List<BankAccount></returns>
        public List<BankAccount> GetAccounts(User user)
        {
            List<BankAccount> accounts = new List<BankAccount>();
            foreach(var account in BankAccounts)
            {
                if(account.Owner == user)
                {
                    accounts.Add(account);
                }
            }
            return accounts;
        }
    }
}
