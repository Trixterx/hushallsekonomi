using hushallsekonomi;
using hushallsekonomi.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace hushallsekonomiUnitTests
{
    [TestFixture]
    public class Ivo
    {
        private User user1;
        private User user2;
        private Bank bank1;
        private BankAccount bankAccount1;
        private BankAccount bankAccount2;

        /// <summary>
        /// Run setup for tests
        /// </summary>
        [SetUp]
        public void Setup()
        {
            user1 = new User() { Firstname = "John", Lastname = "Doe", Salary = 15000 };
            user2 = new User() { Firstname = "Johnny", Lastname = "Bravo", Salary = 0 };

            bankAccount1 = new BankAccount()
            {
                Id = 1,
                Owner = user1,
                Balance = 5000,
                Transactions = new List<Transaction>()
                {
                    new CashTransaction() { Id = 1, TransactionType = TransactionType.In, Sum = 15000 },
                    new CashTransaction() { Id = 2, TransactionType = TransactionType.In, Sum = 20000 },
                    new CashTransaction() { Id = 3, TransactionType = TransactionType.Out, Sum = 20000 },
                }
            };

            bankAccount2 = new BankAccount() { Id = 2, Owner = user2, Balance = 0 };
            bank1 = new Bank()
            {
                Name = "Nordea",
                BankAccounts = new List<BankAccount>() { bankAccount1, bankAccount2, }
            };
            user1.Bank = bank1;
        }

        /// <summary>
        /// Test to see if user can be created with possible parameters
        /// </summary>
        [Test]
        public void TestCreateUser1()
        {
            Assert.AreEqual("John", user1.Firstname);
            Assert.AreEqual("Doe", user1.Lastname);
            Assert.AreEqual(15000, user1.Salary);
            Assert.AreEqual("Nordea", user1.Bank.Name);
        }

        /// <summary>
        /// Test to see if user can be created without using the bank parameter
        /// </summary>
        [Test]
        public void TestCreateUser2()
        {
            Assert.AreEqual("Johnny", user2.Firstname);
            Assert.AreEqual("Bravo", user2.Lastname);
            Assert.AreEqual(0, user2.Salary);
        }

        /// <summary>
        /// Test to see if the sum of all the income is correct and that the method only calculates income
        /// </summary>
        [Test]
        public void TestGetIncomeSumAccount1()
        {
            Assert.AreEqual(35000, bankAccount1.GetIncomeSum());
        }

        /// <summary>
        /// Test to see if the method return zero value when there are no incoming transactions to the account
        /// </summary>
        [Test]
        public void TestGetIncomeSumAccount2()
        {
            Assert.AreEqual(0, bankAccount2.GetIncomeSum());
        }
    }
}