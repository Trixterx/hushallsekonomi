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

            }
            else
            {

            }
        }

        //Ta ut pengar
        public bool Withdraw(List<Transaction> transactions)
        {
            
        }

        //Lägg in pengar
        public bool Deposit(Transaction transaction)
        {

        }

        //Den ska räkna ut summan av inkomst
        public double GetIncomeSum()
        {
            return Transactions.OfType<CashTransaction>().Where(t => t.Sum > 0).Sum(t => t.Sum);
        }

        //Den ska räkna ut summan av utgifter
        public double GetExpenseSum()
        {
            return Transactions.OfType<CashTransaction>().Where(t => t.Sum < 0).Sum(t => t.Sum);
        }

        //Den ska räkna ut summan av beräknade utgifter
        //Den ska tala om hur mycket cash man har över
        public double GetExpensesCalculatedSum()
        {
            return Transactions.OfType<PercentTransaction>().Sum(t => t.Sum);
        }
    }
}
