namespace hushallsekonomi
{
    /// <summary>
    /// The type of transaction a transaction is
    /// <para>Transactions can be of two types, in or out.</para>
    /// <para>Any transfer towards an account made that removes money is of type out, and any deposit is in.</para>
    /// </summary>
    public enum TransactionType
    {
        In,
        Out
    }

    /// <summary>
    /// Transactions are actions made upon an account.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// The Id of this transaction.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The total sum of this transaction. If this is a percentage transaction, this is the percentage describe in full, 10 percent is 10 and not 0.1.
        /// </summary>
        public double Sum { get; set; }
        /// <summary>
        /// What account the transaction was made from
        /// </summary>
        public BankAccount From { get; set; }
        /// <summary>
        /// What account the transaction was made to
        /// </summary>
        public BankAccount To { get; set; }
        /// <summary>
        /// What type of transaction this is.
        /// </summary>
        public TransactionType TransactionType { get; set; }
        /// <summary>
        /// The balance on the account before the transaction has been performed.
        /// </summary>
        public double BalanceOnTransaction { get; set; }
        /// <summary>
        /// The balance on the account after the transaction has been performed.
        /// </summary>
        public double BalanceAfterTransaction { get; set; }
    }
}
