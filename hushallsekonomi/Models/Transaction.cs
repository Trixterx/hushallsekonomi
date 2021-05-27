using System;
using System.Collections.Generic;
using System.Text;

namespace hushallsekonomi
{
    public enum TransactionType
    {
        In,
        Out
    }

    public class Transaction
    {
        public int Id { get; set; }
        public double Sum { get; set; }
        public BankAccount From { get; set; }
        public BankAccount To { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
