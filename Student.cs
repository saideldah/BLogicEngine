using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rule_engine
{
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }

    }
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int AnyNumber { get; set; }
        public string MobileNumber { get; set; }
        public Address Address { get; set; }
        public List<string> SomeList { get; set; }

        public Student()
        {
            Address = new Address();
            SomeList = new List<string>();
        }
    }
}