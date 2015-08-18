using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Bro.ViewModels.Dialogs;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.ProductsViewModels
{
    public class OnStockProductsViewModel : ProductsViewModel
    {
        public OnStockProductsViewModel(MainViewModel model) : base(model.Context)
        {
            _mainViewModel = model;

            AddProductCommand = new DelegateCommand(AddProduct);
            CloseAddDialogCommand = new DelegateCommand(() => AddDialogViewModel = null);

            SellProductCommand = new DelegateCommand(SellProduct, () => SelectedProduct != null && SelectedProduct.Status != TranType.OnRepair);
            EditProductCommand = new DelegateCommand(EditProduct, () => SelectedProduct != null);
            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => SelectedProduct != null);
            RepairProductCommand = new DelegateCommand(RepairProduct, () => SelectedProduct != null && SelectedProduct.Status != TranType.OnRepair);
            TakeProductFromRepairerCommand = new DelegateCommand(TakeProductFromRepairer, () => SelectedProduct != null && SelectedProduct.Status == TranType.OnRepair);
        }

        private readonly MainViewModel _mainViewModel;

        private OnStockProductViewModel _selectedProduct;

        public OnStockProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyPropertyChanged();
                SellProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
                DeleteProductCommand.RaiseCanExecuteChanged();
                RepairProductCommand.RaiseCanExecuteChanged();
                TakeProductFromRepairerCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _closeAddDialogCommand;

        public DelegateCommand CloseAddDialogCommand
        {
            get { return _closeAddDialogCommand; }
            set
            {
                _closeAddDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private AddOnStockProductDialogViewModel _addDialogViewModel;

        public AddOnStockProductDialogViewModel AddDialogViewModel
        {
            get { return _addDialogViewModel; }
            set
            {
                _addDialogViewModel = value;
                NotifyPropertyChanged();
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

        private DelegateCommand _sellProductCommand;

        public DelegateCommand SellProductCommand
        {
            get { return _sellProductCommand; }
            set
            {
                _sellProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _editProductCommand;

        public DelegateCommand EditProductCommand
        {
            get { return _editProductCommand; }
            set
            {
                _editProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _deleteProductCommand;

        public DelegateCommand DeleteProductCommand
        {
            get { return _deleteProductCommand; }
            set
            {
                _deleteProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _repairProductCommand;

        public DelegateCommand RepairProductCommand
        {
            get { return _repairProductCommand; }
            set
            {
                _repairProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _takeProductFromRepairerCommand;

        public DelegateCommand TakeProductFromRepairerCommand
        {
            get { return _takeProductFromRepairerCommand; }
            set
            {
                _takeProductFromRepairerCommand = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Create new transaction with TransactionType "Bought" and add it
        /// </summary>
        public void AddProduct()
        {
            AddDialogViewModel = new AddOnStockProductDialogViewModel(_mainViewModel);
        }

        public void SellProduct()
        {
            //try
            //{
            //    var transaction = new Transaction
            //    {
            //        TypeID = (int) TranType.Sold,
            //        OperatorID = 1,
            //        ProductID = SelectedProduct.ID,
            //        Price = 500,
            //        Date = DateTime.Now,
            //    };

            //    Context.Transactions.Add(transaction);
            //    Context.SaveChanges();

            //    MessageBox.Show("Product sold");
            //}
            //catch (Exception e)
            //{
            //    // TODO: Log exception
            //    MessageBox.Show(string.Format("Failed to sell product: {0}", e.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            MessageBox.Show("Selected product was sold!");
        }

        /// <summary>
        /// Find all transactions with SelectedProduct and delete all of them
        /// </summary>
        public void DeleteProduct()
        {
            MessageBox.Show("Selected product was deleted!");
        }

        /// <summary>
        /// Edit the last transaction with SelectedProduct, but disable to edit TransactionType (Onstock product must stay onstock)
        /// </summary>
        public void EditProduct()
        {
            MessageBox.Show("Selected product was edited!");
        }

        /// <summary>
        /// Add new transaction with SelectedProduct and TypeID "OnRepair"
        /// </summary>
        public void RepairProduct()
        {
            MessageBox.Show("Selected product was given to Repairer!");
        }

        /// <summary>
        /// Add new transaction with SelectedProduct and TypeID "Repaired"
        /// </summary>
        public void TakeProductFromRepairer()
        {
            MessageBox.Show("Selected product was taken from Repairer!");
        }

        protected override List<ProductViewModel> GetProducts(Context context)
        {
            var notSoldProducts =
                context.Products.Where(x => x.Transactions.Any()).ToList()
                    .Where(x => x.Transactions.OrderBy(y => y.Date).Last().TransactionType.ID != (int)TranType.Sold);

            return
                notSoldProducts.Select(x => new OnStockProductViewModel(x))
                    .Where(y => y.Origin == TranType.Bought)
                    .Cast<ProductViewModel>()
                    .ToList();
        }
    }
}
