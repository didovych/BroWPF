using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels.ProductsViewModels
{
    public class ToPawnProductViewModel : ProductViewModel
    {
        public ToPawnProductViewModel(Product product) : base(product)
        {
            var orderedTransactions = product.Transactions.OrderBy(x => x.Date);

            var transactionToPawn = orderedTransactions.LastOrDefault(x => x.TransactionType.ID == (int)TranType.ToPawn);
            if (transactionToPawn != null)
            {
                DateTake = transactionToPawn.Date;
                Client = new ClientViewModel(transactionToPawn.Contragent.Client);
                SalesmanTake = new SalesmanViewModel(transactionToPawn.Operator.Salesman);
            }
        }

        private DateTime _dateTake;

        public DateTime DateTake
        {
            get { return _dateTake; }
            set
            {
                _dateTake = value;
                NotifyPropertyChanged();
            }
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

        private SalesmanViewModel _salesmanTake;

        public SalesmanViewModel SalesmanTake
        {
            get { return _salesmanTake; }
            set
            {
                _salesmanTake = value;
                NotifyPropertyChanged();
            }
        }
    }
}
