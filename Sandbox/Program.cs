using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
            //get the currently logged in user
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(user);
            Console.WriteLine(principal.IsInRole(WindowsBuiltInRole.Administrator));
            Console.ReadLine();

            Context context = new Context();

            using (context)
            {
                ExcelExport excel = new ExcelExport();

                excel.GeneralReport("E:\\report.xlsx", DateTime.MinValue, DateTime.MaxValue, context);

                Console.WriteLine("DONE");
            }
        }
    }
}
