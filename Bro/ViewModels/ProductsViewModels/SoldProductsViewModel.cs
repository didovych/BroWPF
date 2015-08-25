using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows;
using Bro.ViewModels.ProductsViewModels;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels
{
    public class SoldProductsViewModel : ProductsViewModel
    {
        public SoldProductsViewModel(MainViewModel mainViewModel)
            : base(mainViewModel.Context)
        {
            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => SelectedProduct != null);
        }

        private SoldProductViewModel _selectedProduct;

        public SoldProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                if (value != null) SelectedProductID = value.ID;
                NotifyPropertyChanged();
                DeleteProductCommand.RaiseCanExecuteChanged();
            }
        }

        protected override List<ProductViewModel> GetProducts(Context context)
        {
            var products =
                context.Products.Where(x => x.Transactions.Any()).ToList()
                    .Where(x => x.Transactions.OrderBy(y => y.Date).Last().TransactionType.ID == (int)TranType.Sold);

            return products.Select(x => new SoldProductViewModel(x)).Cast<ProductViewModel>().ToList();
        }

        protected override bool Filter(object obj)
        {
            return true;
        }
    }
}
