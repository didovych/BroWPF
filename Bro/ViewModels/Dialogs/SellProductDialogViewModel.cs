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
        public SellProductDialogViewModel(Context context, int selectedProductID, ProductsViewModel productsViewModel)
        {
            _context = context;
            _selectedProductID = selectedProductID;
            _productsViewModel = productsViewModel;

            SellProductCommand = new DelegateCommand(SellProduct, Validate);
        }

        private readonly Context _context;
        private readonly int _selectedProductID;
        private ProductsViewModel _productsViewModel;

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
            // TODO change operatorID
            Transaction transaction = new Transaction { ProductID = _selectedProductID, Date = DateTime.Now, TypeID = (int)TranType.Sold, OperatorID = 1, Price = Price };

            try
            {
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(("Не удалось продать товар."), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Logging.WriteToLog("Failed to sell OnStock product. " + exception.Message);
            }

            _productsViewModel.Update();

            _productsViewModel.SellProductDialogViewModel = null;
        }

        private bool Validate()
        {
            return (Price >= 0);
        }
    }
}
