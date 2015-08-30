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
    public class SellProductDialogViewModel : ViewModelBase
    {
        public SellProductDialogViewModel(MainViewModel mainViewModel, List<int> selectedProductIDs, ProductsViewModel productsViewModel)
        {
            _mainViewModel = mainViewModel;
            _selectedProductIDs = selectedProductIDs;
            _productsViewModel = productsViewModel;

            SellProductCommand = new DelegateCommand(SellProduct, Validate);
        }

        private readonly MainViewModel _mainViewModel;
        private readonly List<int> _selectedProductIDs;
        private readonly ProductsViewModel _productsViewModel;

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyPropertyChanged();
                SellProductCommand.RaiseCanExecuteChanged();
                if (value < 0)
                    MessageBox.Show(("Цена меньше нуля"), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (value == 0)
                    MessageBox.Show(("Цена равна нулю"), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public DelegateCommand SellProductCommand { get; set; }

        private void SellProduct()
        {
            Transaction transaction = new Transaction
            {
                ProductID = _selectedProductIDs.FirstOrDefault(),
                Date = DateTime.Now,
                TypeID = (int) TranType.Sold,
                OperatorID = 1,
                Price = Price
            };

            try
            {
                _mainViewModel.Context.Transactions.Add(transaction);
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(("Не удалось продать товар."), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Logging.WriteToLog("Failed to sell OnStock product. " + exception.Message);
            }


            _mainViewModel.CashInHand += Price;
            var productToSell = _productsViewModel.Products.FirstOrDefault(
                x => x.IDs.FirstOrDefault() == _selectedProductIDs.FirstOrDefault());
            if (productToSell != null) _mainViewModel.ProductsValue -= productToSell.MoneySpentForProduct;

            _productsViewModel.Update();
            _mainViewModel.SoldProductsViewModel.Update();

            _productsViewModel.SellProductDialogViewModel = null;
        }

        private bool Validate()
        {
            return (Price >= 0);
        }
    }
}
