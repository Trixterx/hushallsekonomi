using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hushallsekonomi.Models;

namespace hushallsekonomi
{
    public enum Type
    {
        Savings,
        Expenditure,
        Disaster
    }

    public class BankAccount    //Huvudklass
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public double Balance { get; private set; }

        //Den ska ta emot en lista med alla utgifter, Den ska ta emot en lista med alla inkomster
        public List<Transaction> Transactions { get; set; }
        public Type Type { get; set; }

        //Ta ut pengar
        //Den ska ta emot en lista med alla beräknade utgifter
        //(där anger man inte värde utan procent som ska dras)
        public bool Withdraw(Transaction transaction)
        {
            if (transaction is PercentTransaction)
            {
                // downcast transaction to PercentTransaction to access PercentTransaction-only props and methods
                // save amount in cash
                PercentTransaction t = (PercentTransaction)transaction;
                t.SumInCash = (Balance * (transaction.Sum / 100));

                Balance -= (Balance * (transaction.Sum / 100));
                return true;
            }
            else if (transaction.TransactionType == TransactionType.Out)
            {
                Balance -= transaction.Sum;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the balance of the account to transfer money from is greater or equal to the desired sum to transfer.
        /// </summary>
        /// <param name="p0">The account to transfer money from</param>
        /// <param name="p1">Sum of money to transfer from the account.</param>
        /// <returns>True if money exists, false if there isn't enough money.</returns>
        public bool HasBalance(BankAccount p0, double p1)
        {
            return p0.Balance >= p1;
        }

        //Ta ut pengar
        public void Withdraw(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                Withdraw(transaction);
            }
        }

        //Lägg in pengar
        public bool Deposit(Transaction transaction)
        {
            if (transaction.Sum > 0 && transaction is CashTransaction)
            {
                Balance += transaction.Sum;
                return true;
            }
            return false;
        }

        //Den ska räkna ut summan av inkomst
        public double GetIncomeSum()
        {
            return Transactions.OfType<CashTransaction>().Where(t => t.TransactionType == TransactionType.In).Sum(t => t.Sum);
        }

        //Den ska räkna ut summan av utgifter
        public double GetExpenseSum()
        {
            return Transactions.OfType<CashTransaction>().Where(t => t.TransactionType == TransactionType.Out).Sum(t => t.Sum);
        }

        //Den ska räkna ut summan av beräknade utgifter
        //Den ska tala om hur mycket cash man har över
        public double GetExpensesCalculatedSum()
        {
            return Transactions.OfType<PercentTransaction>().Sum(t => t.SumInCash);
        }

        public bool TransferMoney(double  sum, int fromAccount, int toAccount)
        {
            return false;
        }
    }
}
