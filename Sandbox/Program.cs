using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bro.ViewModels;
using BroData;

namespace Sandbox
{
    class Program
    {
        private static void Main(string[] args)
        {
            using (var context = new Context())
            {
                var trans = context.Transactions.Where(
                    x => x.TransactionType.ID == (int) TranType.Cashin || x.TransactionType.ID == (int) TranType.Cashout)
                    .ToList()
                    .Select(x => new CashTransactionViewModel(x)).ToList();

                Console.WriteLine(trans.Count);

                Console.ReadLine();
            }

            Console.WriteLine();
        }
    }
}
