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
    public class ToRepairProductsViewModel : ProductsViewModel
    {
        public ToRepairProductsViewModel(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddProductCommand = new DelegateCommand(AddProduct);
            CloseAddDialogCommand = new DelegateCommand(() => AddDialogViewModel = null);

            SellProductCommand = new DelegateCommand(SellProduct, () => SelectedProduct != null && SelectedProduct.Status != TranType.OnRepair);
            EditProductCommand = new DelegateCommand(EditProduct, () => SelectedProduct != null);
            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => SelectedProduct != null);

            RepairProductCommand = new DelegateCommand(RepairProduct, () => SelectedProduct != null && SelectedProduct.Status != TranType.OnRepair);
            CloseRepairDialogCommand = new DelegateCommand(() => RepairDialogViewModel = null);

            TakeProductFromRepairerCommand = new DelegateCommand(TakeProductFromRepairer, () => SelectedProduct != null && SelectedProduct.Status == TranType.OnRepair);
            CloseRepairedDialogCommand = new DelegateCommand(() => RepairedDialogViewModel = null);
        }

        private readonly MainViewModel _mainViewModel;
        private ToRepairProductViewModel _selectedProduct;

        public ToRepairProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyPropertyChanged();
                if (value != null) SelectedProductIDs = value.IDs;
                SellProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
                DeleteProductCommand.RaiseCanExecuteChanged();
                RepairProductCommand.RaiseCanExecuteChanged();
                TakeProductFromRepairerCommand.RaiseCanExecuteChanged();
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

        private DelegateCommand _closeRepairDialogCommand;

        public DelegateCommand CloseRepairDialogCommand
        {
            get { return _closeRepairDialogCommand; }
            set
            {
                _closeRepairDialogCommand = value;
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

        private DelegateCommand _closeRepairedDialogCommand;

        public DelegateCommand CloseRepairedDialogCommand
        {
            get { return _closeRepairedDialogCommand; }
            set
            {
                _closeRepairedDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private AddToRepairProductDialogViewModel _addDialogViewModel;

        public AddToRepairProductDialogViewModel AddDialogViewModel
        {
            get { return _addDialogViewModel; }
            set
            {
                _addDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private RepairToRepairProductDialogViewModel _repairDialogViewModel;

        public RepairToRepairProductDialogViewModel RepairDialogViewModel
        {
            get { return _repairDialogViewModel; }
            set
            {
                _repairDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private RepairedToRepairProductDialogViewModel _repairedDialogViewModel;

        public RepairedToRepairProductDialogViewModel RepairedDialogViewModel
        {
            get { return _repairedDialogViewModel; }
            set
            {
                _repairedDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Create new transaction with TransactionType "ToRepair" and add it
        /// </summary>
        public void AddProduct()
        {
            AddDialogViewModel = new AddToRepairProductDialogViewModel(_mainViewModel);
        }  

        /// <summary>
        /// Add new transaction with SelectedProduct and TypeID "OnRepair"
        /// </summary>
        public void RepairProduct()
        {
            RepairDialogViewModel = new RepairToRepairProductDialogViewModel(_mainViewModel);
        }

        /// <summary>
        /// Add new transaction with SelectedProduct and TypeID "Repaired"
        /// </summary>
        public void TakeProductFromRepairer()
        {
            RepairedDialogViewModel = new RepairedToRepairProductDialogViewModel(_mainViewModel);
        }

        protected override List<ProductViewModel> GetProducts(Context context)
        {
            var notSoldProducts =
                context.Products.Where(x => x.Transactions.Any()).ToList()
                    .Where(x => x.Transactions.OrderBy(y => y.Date).Last().TransactionType.ID != (int)TranType.Sold);

            return
                notSoldProducts.Select(x => new ToRepairProductViewModel(x))
                    .Where(y => y.Origin == TranType.ToRepair)
                    .Cast<ProductViewModel>()
                    .ToList();
        }

        protected override bool Filter(object obj)
        {
            return true;
        }
    }
}
