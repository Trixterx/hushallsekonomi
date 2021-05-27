using hushallsekonomi;
using hushallsekonomi.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace hushallsekonomiUnitTests
{
    /// <summary>
    /// Class to perform various bank and bank accounts tests.
    /// </summary>
    public class BankTests
    {
        /// <summary>
        /// We need some banks to make some of our tests
        /// </summary>
        private Bank _bank, _bank2, _bank3;
        /// <summary>
        /// We need some dummy users to make our tests
        /// </summary>
        private User _user, _user2, _userError;
        /// <summary>
        /// We need some bank accounts to make our tests.
        /// </summary>
        private BankAccount _bankAccount, _bankAccount2, _bankAccount3, _bankAccount4;

        /// <summary>
        /// Setup the current tests to be made.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _user = new User() { Firstname = "John", Lastname = "Doe" };
            _user2 = new User() { Firstname = "John", Lastname = "Wick" };
            _userError = new User() { Firstname = "John", Lastname = "Wick" };

            _bankAccount = new BankAccount() { Id = 1, Owner = _user, Balance = 1000};
            _bankAccount2 = new BankAccount() { Id = 2, Owner = _user, Balance = 2000 };
            _bankAccount3 = new BankAccount() { Id = 3, Owner = _user, Balance = 0 };
            _bankAccount4 = new BankAccount() { Id = 4, Owner = _user2};
            _bankAccount4.Deposit(new CashTransaction()
            {
                Sum = 1000,
                TransactionType = TransactionType.In
            });

            _bank = new Bank()
            {
                Name = "Bank 1",
                BankAccounts = new List<BankAccount>() {
                    _bankAccount, _bankAccount2, _bankAccount3, _bankAccount4
                }
            };
            _bank2 = new Bank() { Name = "Bank 2" };
            _bank3 = new Bank() { Name = "Bank 3" };
        }

        /// <summary>
        /// Test and make sure that the transactions stored in the account are correct with the current balance.
        /// </summary>
        [Test]
        public void TestIncomeSum()
        {
            Assert.AreEqual(_bankAccount4.Balance, _bankAccount4.GetIncomeSum());
        }

        /// <summary>
        /// Test and make sure that banks are named correctly from setup.
        /// </summary>
        [Test]
        public void TestBankCreation()
        {
            Assert.AreEqual("Bank 1", _bank.Name);
            Assert.AreEqual("Bank 2", _bank2.Name);
            Assert.AreEqual("Bank 3", _bank3.Name);
        }

        /// <summary>
        /// Run a test on multiple transactions at once.
        /// </summary>
        [Test]
        public void TestMultipleTransactions()
        {
            Assert.AreEqual(true, _bank.BankAccounts[0].Withdraw(new List<Transaction>()
            {
                new Transaction()
                {
                    TransactionType = TransactionType.Out,
                    Sum = 5,
                },
                new Transaction()
                {
                    TransactionType = TransactionType.Out,
                    Sum = 25,
                },
                new Transaction()
                {
                    TransactionType = TransactionType.Out,
                    Sum = 15,
                },
                new Transaction()
                {
                    TransactionType = TransactionType.Out,
                    Sum = 5,
                }
            }));
        }

        /// <summary>
        /// Test opening a bank account
        /// </summary>
        [Test]
        public void OpenBankAccount()
        {
            var user = new User()
            {
                Bank = _bank,
                Firstname = "John",
                Lastname = "Doe",
                Salary = 20000
            };
            Assert.AreEqual(true,
            _bank.OpenBankAccount(
                    user,
                    AccountType.Savings
                ));
            Assert.AreEqual(true,
                _bank.OpenBankAccount(
                    user,
                    AccountType.Expenditure
                ));
            Assert.AreEqual(true,
                _bank.OpenBankAccount(
                    user,
                    AccountType.Disaster
                ));
        }

        /// <summary>
        /// Test if the amount of bank accounts created from this test is the same as the collection, we always expect 4.
        /// </summary>
        [Test]
        public void TestBankAccounts()
        {
            Assert.AreEqual(4, _bank.BankAccounts.Count);
        }

        /// <summary>
        /// Test retrieving how many bank accounts a certain user has
        /// </summary>
        [Test]
        public void TestBankGetAccounts()
        {
            Assert.AreEqual(3, _bank.GetAccounts(_user).Count);
            Assert.AreEqual(1, _bank.GetAccounts(_user2).Count);
            Assert.AreEqual(0, _bank.GetAccounts(_userError).Count);
        }

        /// <summary>
        /// Test receiving a bank account
        /// Test recive 0 if no user added
        /// </summary>
        [Test]
        public void TestBankGetAccount()
        {
            Assert.AreEqual(1, _bank.GetAccounts(_user2).Count);
            Assert.AreEqual(0, _bank.GetAccounts(null).Count);
        }

        /// <summary>
        /// Test transferring money between accounts.
        /// </summary>
        [Test]
        public void TestTransferMoney()
        {
            _bank.TransferMoney(1000, _bankAccount.Id, _bankAccount2.Id);
            Assert.AreEqual(3000, _bankAccount2.Balance);
            Assert.AreEqual(0, _bankAccount.Balance);
            Assert.AreEqual(false, _bank.TransferMoney(1000, _bankAccount.Id, _bankAccount2.Id));
        }
    }
}
