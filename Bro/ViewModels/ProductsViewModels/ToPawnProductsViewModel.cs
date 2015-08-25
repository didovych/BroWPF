using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bro.ViewModels.Dialogs;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.ProductsViewModels
{
    public class ToPawnProductsViewModel : ProductsViewModel
    {
        public ToPawnProductsViewModel(MainViewModel mainViewModel) : base(mainViewModel.Context)
        {
            _mainViewModel = mainViewModel;

            AddProductCommand = new DelegateCommand(AddProduct);
            CloseAddProductCommand = new DelegateCommand(() => AddProductDialogViewModel = null);

            SellProductCommand = new DelegateCommand(SellProduct, () => SelectedProduct != null);
            EditProductCommand = new DelegateCommand(EditProduct, () => SelectedProduct != null);
            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => SelectedProduct != null);
            SendToOnStockProductCommand = new DelegateCommand(SendToOnStockProduct, () => SelectedProduct != null && SelectedProduct.DateSellTo != null && SelectedProduct.DateSellTo.Value.CompareTo(DateTime.Now) <= 0);
        }

        private readonly MainViewModel _mainViewModel;
        private ToPawnProductViewModel _selectedProduct;

        public ToPawnProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                if (value != null) SelectedProductID = value.ID;
                NotifyPropertyChanged();
                SellProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
                DeleteProductCommand.RaiseCanExecuteChanged();
                SendToOnStockProductCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _addProductCommand;

        public DelegateCommand AddProductCommand
        {
            get { return _addProductCommand; }
            set
            {
                _addProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closeAddProductCommand;

        public DelegateCommand CloseAddProductCommand
        {
            get { return _closeAddProductCommand; }
            set
            {
                _closeAddProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _sendToOnStockProductCommand;

        public DelegateCommand SendToOnStockProductCommand
        {
            get { return _sendToOnStockProductCommand; }
            set
            {
                _sendToOnStockProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private AddToPawnProductDialogViewModel _addProductDialogViewModel;

        public AddToPawnProductDialogViewModel AddProductDialogViewModel
        {
            get { return _addProductDialogViewModel; }
            set
            {
                _addProductDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Create new transaction with TransactionType "ToPawn" and add it
        /// </summary>
        public void AddProduct()
        {
            AddProductDialogViewModel = new AddToPawnProductDialogViewModel(_mainViewModel);
        }

        /// <summary>
        /// Add new transaction with Selected product and TransactionType "ToBought"
        /// </summary>
        public void SendToOnStockProduct()
        {
            // TODO change operatorID
            var transaction = new Transaction {ProductID = SelectedProduct.ID, Date = DateTime.Now, TypeID = (int) TranType.Bought, OperatorID = 1, Price = 0};

            try
            {
                _mainViewModel.Context.Transactions.Add(transaction);
                _mainViewModel.Context.SaveChanges();

                MessageBox.Show("Товар поставлен на приход");
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось поставить товар на приход"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed ToPawn -> Bought. " + e.Message);
            }

            Update();
        }

        protected override List<ProductViewModel> GetProducts(Context context)
        {
            var notSoldProducts =
                context.Products.Where(x => x.Transactions.Any()).ToList()
                    .Where(x => x.Transactions.OrderBy(y => y.Date).Last().TransactionType.ID != (int)TranType.Sold);

            return
                notSoldProducts.Select(x => new ToPawnProductViewModel(x))
                    .Where(y => y.Origin == TranType.ToPawn)
                    .Cast<ProductViewModel>()
                    .ToList();
        }

        protected override bool Filter(object obj)
        {
            return true;
        }
    }
}
