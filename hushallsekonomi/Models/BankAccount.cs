using System;
using System.Collections.Generic;
using System.Text;

namespace hushallsekonomi
{
    public enum Type
    {
        Savings,
        Expenditure
    }

    public class BankAccount
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public double Balance { get; private set; }
        public List<Transaction> Transactions { get; set; }
        public Type Type { get; set; }

        public bool Withdraw(double sum)
        {

        }

        public bool Deposit(double sum)
        {

        }

        public double GetIncomeSum()
        {

        }

        public double GetExpenseSum()
        {

        }
    }
}
