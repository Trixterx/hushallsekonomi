using hushallsekonomi;
using hushallsekonomi.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace hushallsekonomiUnitTests
{
    [TestFixture]
    public class Fredrik
    {
        /// <summary>
        /// Random names to use for users.
        /// </summary>
        private readonly List<string> _names = new List<string>
        {
            "Lorem",
            "ipsum",
            "dolor",
            "sit",
            "amet",
            "consectetur",
            "adipiscing",
            "elit",
            "Proin",
            "quis",
            "congue",
            "lectus",
            "Aenean",
            "eleifend",
            "odio",
            "vitae",
            "fringilla",
            "placerat",
            "Pellentesque",
            "at",
            "lacus",
            "ut",
            "neque",
            "aliquet",
            "facilisis",
            "nec",
            "at",
            "purus",
            "Donec",
            "eleifend",
            "eros",
            "et",
            "bibendum",
            "eleifend",
            "justo",
            "sapien",
            "efficitur",
            "tellus",
            "nec",
            "mollis",
            "eros",
            "velit",
            "sed",
            "massa",
            "Nulla",
            "mollis",
            "nunc",
            "consequat",
            "mauris",
            "pretium",
            "pellentesque",
            "Nam",
            "sed",
            "mollis",
            "tortor",
            "Mauris",
            "vel",
            "finibus",
            "tellus"
        };

        /// <summary>
        /// Collection to store all users.
        /// </summary>
        private List<User> _users;

        /// <summary>
        /// Have a bank store all accounts for the GetExpensesCalculatedSum-tests
        /// </summary>
        private Bank _bank;

        /// <summary>
        /// Run setup for tests
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _users = new List<User>();
            _bank = new Bank() { Name = "Grupp 1 Sparbank" };

            for (var i = 0; i < _names.Count - 1; i++)
            {
                _users.Add(new User()
                {
                    Firstname = Helpers.GetARandomName(_names),
                    Lastname = Helpers.GetARandomName(_names),
                    Salary = Helpers.GetARandomSalary(),
                    Bank = _bank
                });
                _bank.OpenBankAccount(_users[^1], AccountType.Savings);
            }
        }

        /*
         *
         *
         *  TESTS
         *
         *
         */

        /// <summary>
        /// Test if it is possible to setup a new user
        /// </summary>
        [Test]
        public void SetupANewUser()
        {
            Assert.AreEqual("Fredrik",Helpers.ToUpperCaseName("fredrik"));
            Assert.AreEqual("Hoffmann", Helpers.ToUpperCaseName("hoffmann"));
            Assert.IsNotEmpty(Helpers.GetARandomName(_names));
            Assert.IsNotEmpty(Helpers.GetARandomName(_names));
            Assert.GreaterOrEqual(Helpers.GetARandomSalary(), 10000d);
            Assert.IsEmpty(Helpers.ToUpperCaseName(null));
        }

        [Test]
        public void ExampleFromPDF()
        {
            //setup
            var b = new Bank();
            var u = new User()
            {
                Firstname = "Fredrik",
                Lastname = "Hoffmann",
                Bank = b,
                Salary = 14500
            };

            //skapa konton
            foreach (var c in Enum.GetValues(typeof(AccountType)))
            {
                b.OpenBankAccount(u, (AccountType)c);
            }

            //plocka ut expenditure-kontot
            var expenditure = b.GetAccounts(u).Find(x => x.Type == AccountType.Expenditure);
            var disaster = b.GetAccounts(u).Find(x => x.Type == AccountType.Disaster);
            var savings = b.GetAccounts(u).Find(x => x.Type == AccountType.Savings);

            if (expenditure != null && disaster != null && savings != null)
            {
                //sätt in månadspengen :)
                expenditure.Deposit(new Transaction() { Sum = u.Salary });

                //alla utgifter
                var transaction = new Transaction()
                {
                    Sum = 8900,
                    TransactionType = TransactionType.Out
                };

                //genomför alla transaktioner
                var transactions = new Transaction[]
                {
                    new Transaction() {Sum = 8900, TransactionType = TransactionType.Out}, //hyra
                    new Transaction() {Sum = 89, TransactionType = TransactionType.Out}, //netflix
                    new Transaction() {Sum = 99, TransactionType = TransactionType.Out}, //mobil
                    new Transaction() {Sum = 199, TransactionType = TransactionType.Out}, //bredband
                    new Transaction() {Sum = 1200, TransactionType = TransactionType.Out}, //mat
                    new Transaction() {Sum = 600, TransactionType = TransactionType.Out}, //förbrukningsvaror
                    new Transaction() {Sum = 45, TransactionType = TransactionType.Out}, //bankavgift
                    new Transaction() {Sum = 1000, TransactionType = TransactionType.Out}, //pension
                    new Transaction() {Sum = 350, TransactionType = TransactionType.Out}, //gym
                    new Transaction() {Sum = 75, TransactionType = TransactionType.Out} //hemförsäkring
                };

                foreach (var tr in transactions) expenditure.Withdraw(tr);

                //genomför överföringar
                var expectSavingsSum = expenditure.Balance * 0.10;
                b.TransferMoney(expectSavingsSum, expenditure.Id, savings.Id); //Spar (10%)
                var expectDisasterSum = expenditure.Balance * 0.25;
                b.TransferMoney(expectDisasterSum, expenditure.Id, disaster.Id); //Oanade utgifter (25%)

                //kontrollera att alla värden är korrekta
                Assert.AreEqual(savings.Balance, expectSavingsSum);
                Assert.AreEqual(disaster.Balance, expectDisasterSum);
                Assert.AreEqual(expenditure.Balance, 1311.525d);
            }
            else
            {
                Assert.Fail("Accounts can't be null");
            }
        }

        /// <summary>
        /// Test if the number of users are equal to the number of provided potential firstnames
        /// </summary>
        [Test]
        public void ShouldHaveAtLeastUsers()
        {
            Assert.GreaterOrEqual(_names.Count, _users.Count);
        }

        /// <summary>
        /// Test if the number of users are equal to the number of provided potential firstnames
        /// </summary>
        [Test]
        public void CalculateTheExpensesOfPercentTransactions()
        {
            var account = _bank.GetAccounts(_users[new Random().Next(0, _users.Count - 1)])[0];
            //provide money for the account so that we can perform the test
            if (account.Balance < 1)
            {
                account.Balance += new Random().Next(1, int.MaxValue);
            }
            account.Withdraw(new PercentTransaction()
            {
                Sum = 10,
                TransactionType = TransactionType.Out
            });
            Assert.AreEqual(account.Balance * 0.1, account.GetExpensesCalculatedSum());
        }
    }
}
