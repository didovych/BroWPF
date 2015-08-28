using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BroData;

namespace Bro.ViewModels.Dialogs
{
    public class ShowAllTransactionsDialogViewModel: ViewModelBase
    {
        public ShowAllTransactionsDialogViewModel(Context context, List<int> ids, ProductsViewModel productsViewModel)
        {
            var transactions = new ObservableCollection<TransactionViewModel>(GetTransactions(ids, context));
            TransactionsView = CollectionViewSource.GetDefaultView(transactions);
        }

        private ICollectionView _transactionsView;

        public ICollectionView TransactionsView
        {
            get { return _transactionsView; }
            set
            {
                _transactionsView = value;
                NotifyPropertyChanged();
            }
        }

        private List<TransactionViewModel> GetTransactions(List<int> ids, Context context)
        {
            List<TransactionViewModel> result = new List<TransactionViewModel>();

            foreach (var id in ids)
            {
                var product = context.Products.FirstOrDefault(x => x.ID == id);
                if (product == null) continue;
                if (product.Transactions == null) continue;
                result.AddRange(product.Transactions.OrderBy(t => t.Date).ToList().Select(t => new TransactionViewModel(t)).ToList());
            }

            return result;
        }
    }
}
