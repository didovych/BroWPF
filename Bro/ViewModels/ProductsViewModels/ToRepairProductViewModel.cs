using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels.ProductsViewModels
{
    public class ToRepairProductViewModel : ProductViewModel
    {
        public ToRepairProductViewModel(Product product) : base(product)
        {
            var orderedTransactions = product.Transactions.OrderBy(x => x.Date);

            var lastTransaction = orderedTransactions.LastOrDefault();
            if (lastTransaction != null)
            {
                LastTransactionDate = lastTransaction.Date;
                LastTransactionSalesman = new SalesmanViewModel(lastTransaction.Operator.Salesman);

                CurrentRepairer = (lastTransaction.Contragent == null || lastTransaction.Contragent.Repairer == null)  ? null : new RepairerViewModel(lastTransaction.Contragent.Repairer);
            }

            var ToRepairTransaction = orderedTransactions.LastOrDefault(x => x.TransactionType.ID == (int)TranType.ToRepair);
            if (ToRepairTransaction != null)
                Client = new ClientViewModel(ToRepairTransaction.Contragent.Client);
        }

        private ClientViewModel _client;

        public ClientViewModel Client
        {
            get { return _client; }
            set
            {
                _client = value;
                NotifyPropertyChanged();
            }
        }

        private RepairerViewModel _currentRepairer;

        public RepairerViewModel CurrentRepairer
        {
            get { return _currentRepairer; }
            set
            {
                _currentRepairer = value;
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

        private SalesmanViewModel _lastTransactionSalesman;

        public SalesmanViewModel LastTransactionSalesman
        {
            get { return _lastTransactionSalesman; }
            set
            {
                _lastTransactionSalesman = value;
                NotifyPropertyChanged();
            }
        }
    }
}
