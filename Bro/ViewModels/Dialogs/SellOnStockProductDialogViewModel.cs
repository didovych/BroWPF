using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bro.ViewModels.ProductsViewModels;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class SellOnStockProductDialogViewModel : ViewModelBase
    {
        public SellOnStockProductDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _productToSell = mainViewModel.OnStockProductsViewModel.SelectedProduct;

            SellProductCommand = new DelegateCommand(SellProduct, Validate);
        }

        private readonly MainViewModel _mainViewModel;
        private readonly OnStockProductViewModel _productToSell;

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyPropertyChanged();
                SellProductCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SellProductCommand { get; set; }

        private void SellProduct()
        {
            if (_productToSell == null)
            {
                MessageBox.Show(("Не удалось продать товар. Товар не выбран."), "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed to sell OnStock product. Product was not selected.");
                _mainViewModel.OnStockProductsViewModel.RepairedDialogViewModel = null;
            }

            // TODO change operatorID
            Transaction transaction = new Transaction { ProductID = _productToSell.ID, Date = DateTime.Now, TypeID = (int)TranType.Sold, OperatorID = 1, Price = Price };

            try
            {
                _mainViewModel.Context.Transactions.Add(transaction);
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(("Не удалось продать товар."), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Logging.WriteToLog("Failed to sell OnStock product." + exception.Message);
            }

            _mainViewModel.OnStockProductsViewModel.SellDialogViewModel = null;
        }

        private bool Validate()
        {
            return (Price >= 0);
        }
    }
}
