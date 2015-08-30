using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bro.ViewModels;
using Bro.ViewModels.MobileTransactions;
using Bro.ViewModels.Transactions;
using BroData;

namespace Bro.Helpers
{
    public class ReportHelper
    {
        public static List<SoldTransactionViewModel> GetSoldTransactions(DateTime fromDate, DateTime throughDate,
            Context context)
        {
            return
                context.Transactions.Where(
                    x => x.Date >= fromDate && x.Date <= throughDate && x.TypeID == (int)TranType.Sold).OrderBy(x => x.Date)
                    .ToList()
                    .Select(x => new SoldTransactionViewModel(x))
                    .ToList();
        }

        public static List<SoldTransactionViewModel> GetSoldTransactions(int salesmanID, DateTime fromDate,
            DateTime throughDate,
            Context context)
        {
            var result = GetSoldTransactions(fromDate, throughDate, context);

            return result.Where(x => x.Salesman.ID == salesmanID).ToList();
        }

        public static List<CashTransactionViewModel> GetSalaryTransactions(DateTime fromDate, DateTime throughDate,
            Context context)
        {
            return
                context.Transactions.Where(
                    x => x.Date >= fromDate && x.Date <= throughDate && x.TypeID == (int)TranType.Salary).OrderBy(x => x.Date)
                    .ToList()
                    .Select(x => new CashTransactionViewModel(x))
                    .ToList();
        }

        public static List<CashTransactionViewModel> GetSalaryTransactions(int employeeID, DateTime fromDate,
            DateTime throughDate, Context context)
        {
            var result = GetSalaryTransactions(fromDate, throughDate, context);

            return result.Where(x => x.Contragent != null && x.Contragent.ID == employeeID).ToList();
        }

        public static List<CashTransactionViewModel> GetCashTransactions(DateTime fromDate,
            DateTime throughDate, Context context)
        {
            return
                context.Transactions.Where(
                    x =>
                        x.Date >= fromDate && x.Date <= throughDate &&
                        (x.TypeID == (int) TranType.CashIn || x.TypeID == (int) TranType.CashOut))
                    .OrderBy(x => x.Date)
                    .ToList()
                    .Select(x => new CashTransactionViewModel(x))
                    .ToList();
        }

        public static List<CashTransactionViewModel> GetCashTransactions(int salesmanID, DateTime fromDate,
            DateTime throughDate, Context context)
        {
            return GetCashTransactions(fromDate, throughDate, context).Where(x => x.Salesman.ID == salesmanID).ToList();
        }

        public static List<MobileTransactionViewModel> GetMobileTransactions(DateTime fromDate,
            DateTime throughDate, Context context)
        {
            return
                context.MobileTransactions.Where(
                    x => x.Transaction.Date >= fromDate && x.Transaction.Date <= throughDate)
                    .ToList()
                    .Select(x => new MobileTransactionViewModel(x))
                    .ToList();
        }
    }
}
