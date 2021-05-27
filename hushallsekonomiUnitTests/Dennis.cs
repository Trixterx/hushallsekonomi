using hushallsekonomi;
using hushallsekonomi.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Transaction = hushallsekonomi.Transaction;

namespace hushallsekonomiUnitTests
{
    public class Dennis
    {
        private BankAccount _bankAccount;

        [SetUp]
        public void Setup()
        {
            _bankAccount = new BankAccount()
            {
                Transactions = new List<Transaction>()
                {
                    new CashTransaction() { Id = 1, TransactionType = TransactionType.In, Sum = 100 },
                    new CashTransaction() { Id = 2, TransactionType = TransactionType.In, Sum = 200 },
                    new CashTransaction() { Id = 3, TransactionType = TransactionType.Out, Sum = 300 },
                    new CashTransaction() { Id = 4, TransactionType = TransactionType.Out, Sum = 400 }
                }
            };
        }

        [Test]
        public void TestTransactionIn()
        {
            Assert.AreEqual(300d, _bankAccount.Transactions.Where(t => t.TransactionType == TransactionType.In).Sum(t => t.Sum));
        }

        [Test]
        public void TestTransactionOut()
        {
            Assert.AreEqual(700d, _bankAccount.Transactions.Where(t => t.TransactionType == TransactionType.Out).Sum(t => t.Sum));
        }

        [Test]
        public void TestNumberOfTransactionsIn()
        {
            Assert.AreEqual(2, _bankAccount.Transactions.Count(t => t.TransactionType == TransactionType.In));
        }

        [Test]
        public void TestNumberOfTransactionsOut()
        {
            Assert.AreEqual(2, _bankAccount.Transactions.Count(t => t.TransactionType == TransactionType.Out));
        }

        [Test]
        public void TestNumberOfTransactionsTotal()
        {
            Assert.AreEqual(4, _bankAccount.Transactions.Count);
        }

        [Test]
        public void TestDeposit()
        {
            Assert.AreEqual(true, _bankAccount.Deposit(_bankAccount.Transactions[1]));
        }

        [Test]
        public void TestWithdraw()
        {
            Assert.AreEqual(false, _bankAccount.Withdraw(_bankAccount.Transactions[2]));
        }

        /// <summary>
        /// Test GetExpenseSum
        /// </summary>
        [Test]
        public void TestExpenseSum()
        {
            Assert.AreEqual(700d, _bankAccount.GetExpenseSum());
        }
    }
}