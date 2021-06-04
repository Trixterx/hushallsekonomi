using hushallsekonomi.Models;
using hushallsekonomiClasses.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hushallsekonomi
{
    /// <summary>
    /// A bank is a manager of bank accounts
    /// <see cref="BankAccounts"/>
    /// </summary>
    public class Bank
    {
        /// <summary>
        /// The name of this bank.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// All bank accounts belonging to this bank.
        /// </summary>
        public List<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

        /// <summary>
        /// Get all bank accounts for specific user
        /// </summary>
        /// <param name="user">specified user</param>
        /// <returns>List of BankAccounts</returns>
        public List<BankAccount> GetAccounts(User user)
        {
            var accounts = new List<BankAccount>();
            if (user is null)
            {
                Logging.Log("No available accounts");
            }
            else
            {
                try
                {
                    accounts.AddRange(BankAccounts.Where(account => account.Owner == user));
                }
                catch (Exception e)
                {
                    Logging.Log(e);
                }
            }
            return accounts;
        }

        /// <summary>
        /// Open a new bankaccount for a user
        /// </summary>
        /// <param name="owner">The owner of the new bank account</param>
        /// <param name="type">The type of bank account</param>
        /// <returns>True for if the account could be created or false if not.</returns>
        public bool OpenBankAccount(User owner, AccountType type)
        {
            var amountOfAccounts = BankAccounts.Count;
            if (owner is null)
                return false;

            BankAccounts.Add(new BankAccount()
            {
                Balance = 0,
                Id = BankAccounts.Count,
                Owner = owner,
                Transactions = new List<Transaction>(),
                Type = type
            });

            return BankAccounts.Count > amountOfAccounts;
        }

        /// <summary>
        /// Transfer money from account to account
        /// </summary>
        /// <param name="sum">amount of money</param>
        /// <param name="fromAccId">from account id</param>
        /// <param name="toAccId">to account id</param>
        /// <returns>True if the money transfer is successfull</returns>
        public bool TransferMoney(double sum, int fromAccId, int toAccId)
        {
            var fromAccount = GetAccount(fromAccId);
            var toAccount = GetAccount(toAccId);

            try
            {
                if (HasBalance(fromAccount, sum))
                {
                    fromAccount.Balance -= sum;
                    toAccount.Balance += sum;
                    
                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.Log(e);
            }
            return false;
        }

        /// <summary>
        /// Transfer money from account to account
        /// </summary>
        /// <param name="sum">amount of money</param>
        /// <param name="fromAccId">from account id</param>
        /// <param name="toAccId">to account id</param>
        /// <returns>True if the money transfer is successfull</returns>
        public bool TransferMoney(Transaction transaction, int fromAccId, int toAccId)
        {
            var fromAccount = GetAccount(fromAccId);
            var toAccount = GetAccount(toAccId);

            var sum = transaction.Sum;

            if(transaction is PercentTransaction)
            {
                sum = fromAccount.Balance * sum;
            }

            try
            {
                if (HasBalance(fromAccount, sum))
                {
                    fromAccount.Balance -= sum;
                    toAccount.Balance += sum;
                    transaction.From = fromAccount;
                    transaction.To = toAccount;
                    fromAccount.Transactions.Add(transaction);

                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.Log(e);
            }
            return false;
        }

        /// <summary>
        /// Check if the balance of the account to transfer money from is greater or equal to the desired sum to transfer.
        /// </summary>
        /// <param name="account">The account to transfer money from</param>
        /// <param name="moneyToWithdraw">Sum of money to transfer from the account</param>
        /// <returns>True if money exists, false if there isn't enough money.</returns>
        private bool HasBalance(BankAccount account, double moneyToWithdraw)
        {
            try
            {
                if (account.Balance >= moneyToWithdraw)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.Log(e);
            }
            return false;
        }

        /// <summary>
        /// get bank account by id
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>bank account if success, null if false</returns>
        private BankAccount GetAccount(int id)
        {
            try
            {
                foreach (var bankAccount in BankAccounts.Where(bankAccount => bankAccount.Id == id))
                {
                    return bankAccount;
                }
            }
            catch (Exception e)
            {
                Logging.Log(e);
            }
            return null;
        }
    }
}
