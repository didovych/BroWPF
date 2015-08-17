using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.ProductsViewModels
{
    public class OnStockProductViewModel : ProductViewModel
    {
        public OnStockProductViewModel(Product product): base(product)
        {
            var orderedTransactions = product.Transactions.OrderBy(x => x.Date);

            var transactionBought = orderedTransactions.LastOrDefault(x => x.TransactionType.ID == (int) TranType.Bought);
            if (transactionBought != null)
            {
                DateBought = transactionBought.Date;
                SalesmanBought = new SalesmanViewModel(transactionBought.Operator.Salesman);
            }

            var lastTransaction = orderedTransactions.LastOrDefault();
            if (lastTransaction != null)
            {
                LastTransactionDate = lastTransaction.Date;
            }
        }

        private DateTime _dateBought;

        public DateTime DateBought
        {
            get { return _dateBought; }
            set
            {
                _dateBought = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _lastTransactionDate;

        public DateTime LastTransactionDate
        {
            get { return _lastTransactionDate; }
            set
            {
                _lastTransactionDate = value;
                NotifyPropertyChanged();
            }
        }

        private SalesmanViewModel _salesmanBought;

        public SalesmanViewModel SalesmanBought
        {
            get { return _salesmanBought; }
            set
            {
                _salesmanBought = value;
                NotifyPropertyChanged();
            }
        }

    }
}
