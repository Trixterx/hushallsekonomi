using hushallsekonomi.Models;
using hushallsekonomiClasses.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace hushallsekonomi
{
    /// <summary>
    /// BankAccount Type
    /// </summary>
    public enum AccountType
    {
        Savings,
        Expenditure,
        Disaster
    }

    /// <summary>
    /// A bank account is a personal account where transactions, money and owner is stored.
    /// </summary>
    public class BankAccount
    {
        /// <summary>
        /// The Id of this account.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The current owner of this account.
        /// </summary>
        public User Owner { get; set; }

        /// <summary>
        /// The running balance of this account.
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// All transactions made to this account.
        /// </summary>
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        /// <summary>
        /// The type of bank account this is.
        /// </summary>
        public AccountType Type { get; set; }

        private List<string> ReportContent { get; set; } = new List<string>();

        /// <summary>
        /// Make a withdrawal.
        /// </summary>
        /// <param name="transaction">A transaction of type out</param>
        /// <returns>True if all conditions are met</returns>
        public bool Withdraw(Transaction transaction)
        {
            try
            {
                if (transaction.Sum < 0)
                {
                    Logging.Log("A transaction for withdrawal must be made larger than that of 0, provided: " +
                                transaction.Sum + ".");
                    return false;
                }

                if (transaction.TransactionType != TransactionType.Out)
                {
                    Logging.Log("A transaction for withdrawal must be made in a type where type is 'Out'.");
                    return false;
                }

                if (transaction is PercentTransaction t)
                {
                    if (t.Sum > 100)
                    {
                        Logging.Log("A transaction of type percentage can't be larger than the total (" + t.Sum +
                                    "% was provided).");
                        return false;
                    }

                    t.BalanceOnTransaction = Balance;

                    Balance -= t.SumInCash;
                    t.SumInCash = (Balance * (transaction.Sum / 100));
                    if (!(Balance >= transaction.Sum))
                    {
                        Logging.Log(
                            "A transaction of type percentage can't result in a larger grand total withdraw than the total account balance. Balance: " +
                            Balance + ", provided percentage: " + t.Sum + "%, grand total withdrawal: " + t.SumInCash +
                            " SEK.");
                        return false;
                    }

                    t.BalanceAfterTransaction = Balance;
                    t.From = this;
                    Transactions.Add(transaction);
                    return true;
                }

                transaction.BalanceOnTransaction = Balance;
                if (!(Balance >= transaction.Sum))
                {
                    Logging.Log(
                        "A transaction of type cashtransaction can't result in a larger grand total withdraw than the total account balance. Balance: " +
                        Balance + ", provided cash total withdrawal: " + transaction.Sum + " SEK.");
                    return false;
                }

                Balance -= transaction.Sum;
                transaction.BalanceAfterTransaction = Balance;
                transaction.From = this;
                Transactions.Add(transaction);
                return true;
            }
            catch (Exception e)
            {
                Logging.Log(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Make a withdrawal.
        /// </summary>
        /// <param name="transactions">Transactions to make</param>
        /// <returns>True if the transaction can be made from the account</returns>
        public bool Withdraw(List<Transaction> transactions)
        {
            return transactions.All(Withdraw);
        }

        /// <summary>
        /// Deposit money
        /// </summary>
        /// <param name="transaction">A transaction containing relevant data for the deposit</param>
        /// <returns>True if the transaction can be made to the account</returns>
        public bool Deposit(Transaction transaction)
        {
            if (transaction is null)
            {
                Logging.Log("A transaction cannot be performed. Transaction value is NULL");
                return false;
            }
            if (transaction is PercentTransaction)
            {
                Logging.Log("A transaction for deposit can't be of other type than CashTransaction, transaction provided: " + transaction.TransactionType + ", sum: " + transaction.Sum + ", id: " + transaction.Id + ".");
                return false;
            }
            if (transaction.Sum < 0)
            {
                Logging.Log("A transaction for deposit can't be made with a grand total of less than 0, provided: " + transaction.Sum + ".");
                return false;
            }

            Balance += transaction.Sum;
            Transactions.Add(transaction);
            ReportContent.Add($"<tr><td>{transaction.Message}</td><td style=\"text-align:right\">{transaction.Sum}</td><td></td><td></td><td></td></tr>");
            return true;
        }

        /// <summary>
        /// Run the prepare to delete earlier report and write the basic header to it. <see cref="WriteReportHeaders"/>
        /// </summary>
        public void PrepareBudgetReport()
        {
            File.Delete(Logging.BudgetReportPath);
            WriteReportHeaders();
        }

        /// <summary>
        /// Write all transaction data to the budget report collection
        /// </summary>
        private void WriteBudgetExpenses()
        {
            var transactions = Transactions.Where(x => x.TransactionType == TransactionType.Out && !(x is PercentTransaction));

            if (transactions != null)
            {
                foreach (var tr in transactions)
                {
                    ReportContent.Add($"<tr><td>{tr.Message}</td><td></td><td style=\"text-align:right\">{tr.Sum}</td><td></td><td></td></tr>");
                }
            }
        }

        /// <summary>
        /// Write all calculated transaction data to the budget report collection
        /// </summary>
        private void WriteCalculatedBudgetReport()
        {
            var transactions = Transactions.Where(x => x.TransactionType == TransactionType.Out && x is PercentTransaction);

            if (transactions != null)
            {
                foreach (var tr in transactions)
                {
                    ReportContent.Add($"<tr><td>{tr.To.Type.ToString()} ({tr.Sum * 100}%)</td><td></td><td></td><td style=\"text-align:right\">{tr.Sum * 100}%</td><td></td></tr>");
                }
            }
        }

        /// <summary>
        /// Write necessary html headers to the budget report file.
        /// </summary>
        private void WriteReportHeaders()
        {
            Logging.Write(new string[] {
                "<!DOCTYPE html>",
                "<html lang=\"en\">",
                "<head>",
                "<style>",
                "table{ border-collapse: collapse} table tbody tr:nth-child(odd){background:#f4f4f4} table tbody td{border:solid 1px #ccc}",
                "</style>",
                "</head>",
                "<body>",
                "<table style=\"width:40%;max-width:400px;margin:0 auto\"><thead><tr style=\"font-weight: bolder\">",
                "<td>Post",
                "</td><td>Inkomst",
                "</td><td>Utgift",
                "</td><td>Utgift %",
                "</td><td>Kvar</td>",
                "</tr>",
                "<tbody>",
                "<tr>",
                "<td style=\"font-weight: bolder\">Inkomst</td><td></td><td></td><td></td><td></td>",
                "</tr>"
            });
        }

        /// <summary>
        /// Concatenate all earlier results from working on this account
        /// </summary>
        public void CloseReport()
        {
            WriteBudgetExpenses();
            ReportContent.Add("<tr><td style=\"font-weight: bolder\">Kalkylerade utgifter</td><td></td><td></td><td></td><td></td></tr>");
            WriteCalculatedBudgetReport();
            ReportContent.Add($"<tr><td style=\"font-weight: bolder\">Cash kvar:</td><td colspan=\"3\"></td><td style=\"text-align:right;font-weight:bolder\">{Balance}</td></tr>");
            ReportContent.AddRange(new string[] {
                "</tbody></table>",
                "</html>"
            }); 

            foreach(string s in ReportContent)
            {
                Logging.Write(s);
            }
        }

        /// <summary>
        /// Get the grand total sum of all transactions made.
        /// Value is calculated for the transaction type cash and in
        /// </summary>
        /// <returns>The sum of all the income transactions</returns>
        public double GetIncomeSum()
        {
            return Transactions.OfType<CashTransaction>().Where(t => t.TransactionType == TransactionType.In).Sum(t => t.Sum);
        }

        /// <summary>
        /// Get all expenses made through cash transactions
        /// </summary>
        /// <returns>The sum of all the expenses</returns>
        public double GetExpenseSum()
        {
            return Transactions.OfType<CashTransaction>().Where(t => t.TransactionType == TransactionType.Out).Sum(t => t.Sum);
        }

        /// <summary>
        /// Get all expenses made via percentage transactions
        /// </summary>
        ///<returns>The sum of all the calculated (percentage) expenses</returns>
        public double GetExpensesCalculatedSum()
        {
            return Transactions.OfType<PercentTransaction>().Sum(t => t.SumInCash);
        }
    }
}
