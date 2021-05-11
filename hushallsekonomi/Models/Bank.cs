using System;
using System.Collections.Generic;
using System.Text;

namespace hushallsekonomi
{
    public class Bank
    {
        public string Name { get; set; }
        public List<BankAccount> BankAccounts { get; set; }

        public List<BankAccount> GetAccounts(User user)
        {

        }
    }
}
