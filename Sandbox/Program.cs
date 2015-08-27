using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bro.ViewModels;
using Bro.ViewModels.MobileTransactions;
using BroData;

namespace Sandbox
{
    class Program
    {
        private static void Main(string[] args)
        {
            using (var context = new Context())
            {
                var operators = context.MobileOperators;//.Select(x => new MobileOperatorViewModel(x)).ToList();

                Console.WriteLine(operators.ToList().Select(x => new MobileOperatorViewModel(x)).ToList().Count + "");

                Console.ReadLine();
            }
        }
    }
}
