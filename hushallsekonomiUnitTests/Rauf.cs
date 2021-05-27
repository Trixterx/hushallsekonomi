using hushallsekonomi;
using hushallsekonomi.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace hushallsekonomiUnitTests
{
    // By Rauf Karimli
    public class Rauf
    {
        private Transaction deposit;
        private Transaction depositNegValue;
        private BankAccount bankAccount1;
        private BankAccount bankAccount2;


        [SetUp]
        public void Setup()
        {
            bankAccount1 = new BankAccount();
            bankAccount2 = new BankAccount();
            deposit = new CashTransaction() { Id = 1, Sum = 100, From = bankAccount1, To = bankAccount2, TransactionType = TransactionType.In };
           depositNegValue = new CashTransaction() { Id = 1, Sum = -100, From = bankAccount1, To = bankAccount2, TransactionType = TransactionType.In };
        }

        /// <summary>
        /// Tests deposit cash transaction with positiv value
        /// </summary>
        [Test]

        public void Test_Deposit()
        {
            var ba = new BankAccount();
            var transaction = deposit;
            bool actual = ba.Deposit(transaction);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests deposit cash transaction = null
        /// </summary>

        [Test]

        public void Test_DepositNull()
        {
            var ba = new BankAccount();
            bool actual = ba.Deposit(null);
            var expected = false;
            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Tests deposit cash transaction with negtive value
        /// </summary>
        [Test]

        public void Test_DepositNegativeValue()
        {
            var ba = new BankAccount();
            bool actual = ba.Deposit(depositNegValue);
            var expected = false;
            Assert.AreEqual(expected, actual);
        }
    }
}