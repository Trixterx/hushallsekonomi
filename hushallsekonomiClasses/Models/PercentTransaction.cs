namespace hushallsekonomi.Models
{
    /// <summary>
    /// A percentage transaction is a withdrawal alternative to a regular transaction.
    /// </summary>
    public class PercentTransaction : Transaction
    {
        /// <summary>
        /// SumInCash is the total sum the percentage had when the transaction was performed.
        /// </summary>
        public double SumInCash { get; set; }
    }
}
