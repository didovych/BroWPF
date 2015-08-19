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
                int? lastRepairerID = null;
                lastRepairerID =
                context.Transactions.OrderBy(x => x.Date).ToList().LastOrDefault(x => x.ProductID == 15 && x.TypeID == (int)TranType.OnRepair).ContragentID;

                Console.WriteLine(lastRepairerID);

                Console.ReadLine();
            }
        }
    }
}
