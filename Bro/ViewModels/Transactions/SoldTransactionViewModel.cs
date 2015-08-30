using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels.Transactions
{
    public class SoldTransactionViewModel: TransactionViewModel
    {
        public SoldTransactionViewModel(Transaction transaction) : base(transaction)
        {
            if (Type == TranType.Sold && transaction.Product != null) SoldProduct = new SoldProductViewModel(transaction.Product);
        }

        private SoldProductViewModel _soldProduct;

        public SoldProductViewModel SoldProduct
        {
            get { return _soldProduct; }
            set
            {
                _soldProduct = value;
                NotifyPropertyChanged();
            }
        }
    }
}
