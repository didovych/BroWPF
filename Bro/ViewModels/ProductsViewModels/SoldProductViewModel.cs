using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels
{
    public class SoldProductViewModel : ProductViewModel
    {
        public SoldProductViewModel(Product product) : base(product)
        {
            var ordered = product.Transactions.OrderBy(x => x.Date);

            var transactionSold = ordered.LastOrDefault(x => x.TransactionType.ID == (int)TranType.Sold);
            if (transactionSold != null)
            {
                if (transactionSold.Price != null) PriceSold = transactionSold.Price.Value;
                SalesmanSold = new SalesmanViewModel(transactionSold.Operator.Salesman);
                DateSold = transactionSold.Date;
            }

            Profit = PriceSold - MoneySpentForProduct;
        }

        private decimal _priceSold;

        public decimal PriceSold
        {
            get { return _priceSold; }
            set
            {
                _priceSold = value;
                NotifyPropertyChanged();
            }
        }

        private SalesmanViewModel _salesmanSold;

        public SalesmanViewModel SalesmanSold
        {
            get { return _salesmanSold; }
            set
            {
                _salesmanSold = value; 
                NotifyPropertyChanged();
            }
        }

        private decimal _profit;

        public decimal Profit
        {
            get { return _profit; }
            set
            {
                _profit = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _dateSold;

        public DateTime DateSold
        {
            get { return _dateSold; }
            set
            {
                _dateSold = value;
                NotifyPropertyChanged();
            }
        }
    }
}
