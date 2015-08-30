using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bro.Helpers;
using Bro.ViewModels;
using Bro.ViewModels.MobileTransactions;
using Bro.ViewModels.Transactions;
using BroData;

namespace Sandbox
{
    class Program
    {
        private static void Main(string[] args)
        {
            Context context = new Context();

            using (context)
            {
                ExcelExport excel = new ExcelExport();

                excel.SalesmanReport("E:\\report.xlsx", 1, DateTime.MinValue, DateTime.MaxValue, context);

                Console.WriteLine("DONE");
                Console.ReadLine();
            }
        }
    }
}
