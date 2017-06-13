using RuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rule_engine
{
    class Program
    {
        static void Main(string[] args)
        {
            Student s = new Student();
            s.Name = "saeed";
            s.Age = 29;
            s.AnyNumber = 55;
            s.MobileNumber = "76796826";
            s.Address.City = "Beirut";
            s.Address.Street = "Street 12233";
            s.SomeList.Add("saeeed");

            string exp = "object.SomeList.Count() > 0";


            bool valid = RuleManager.Evaluate(exp, s);

            Console.WriteLine(valid);
        }
    }
}
