namespace hushallsekonomi
{
    /// <summary>
    /// User class with user data. Owner of bank account
    /// </summary>
    public class User
    {
        /// <summary>
        /// First name of the user
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Users salary
        /// </summary>
        public double Salary { get; set; }

        /// <summary>
        /// Users bank
        /// </summary>
        public Bank Bank { get; set; }
    }
}
