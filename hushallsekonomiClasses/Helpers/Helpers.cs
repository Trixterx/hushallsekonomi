namespace hushallsekonomi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Helpers
    {
        /// <summary>
        /// Picks a random name from a collection
        /// </summary>
        /// <returns>Random name</returns>
        public static string GetARandomName(List<string> names) => ToUpperCaseName(names[GetARandomIntUsingNamesAsTopValue(names.Count - 1)]);

        /// <summary>
        /// Return a random integer starting from 10'000.
        /// </summary>
        /// <returns>Value interval between 10'000 - 17'500</returns>
        public static int GetARandomSalary() => new Random().Next(10000, 37500);

        /// <summary>
        /// Return a random integer based on the child items from a collection
        /// </summary>
        /// <returns>Random integer starting from 0 as lowest.</returns>
        public static int GetARandomIntUsingNamesAsTopValue(int max) => new Random().Next(0, max);

        /// <summary>
        /// Takes a string and makes the first character uppercase
        /// <para>You should only provide a string longer than 1 character.</para>
        /// </summary>
        /// <param name="name">The string you wish to </param>
        /// <returns>firstname as Firstname.</returns>
        public static string ToUpperCaseName(string name)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;
            if (name.Length > 0)
            {
                return name[0].ToString().ToUpper() + name[1..];
            }
            return name;
        }

        public static int LongestMessage(Transaction[] p0)
        {
            if (p0 != null)
            {
                return p0.OrderByDescending(s => s.Message.Length).First().Message.Length;
            }
            return 0;
        }

        public static int LongestSum(Transaction[] p0)
        {
            if (p0 != null)
            {
                return p0.OrderByDescending(s => s.Sum.ToString().Length).First().Sum.ToString().Length;
            }
            return 0;
        }
    }
}
