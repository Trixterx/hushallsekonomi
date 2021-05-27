using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace hushallsekonomi
{
    public class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public double Salary { get; set; }
        public Bank Bank { get; set; }
    }
}
