using System;
using System.Collections.Generic;
using System.Text;

namespace hushallsekonomi
{
    public class Transaction
    {
        public int Id { get; set; }
        public double Sum { get; set; }

        /// <summary>
        /// true = income
        /// </summary>
        public bool Type { get; set; }
    }
}
