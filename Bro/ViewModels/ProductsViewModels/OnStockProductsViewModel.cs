using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Bro.Helpers;
using Bro.Services;
using Bro.ViewModels.Dialogs;
using BroData;
using Microsoft.Practices.Prism.Commands;
using MessageBox = System.Windows.MessageBox;

namespace Bro.ViewModels.ProductsViewModels
{
    public class OnStockProductsViewModel : ProductsViewModel
    {
        public OnStockProductsViewModel(MainViewModel model) : base(model)
        {
            _mainViewModel = model;

            AddProductCommand = new DelegateCommand(AddProduct);
            CloseAddDialogCommand = new DelegateCommand(() => AddDialogViewModel = null);

            SellProductCommand = new DelegateCommand(SellProduct, () => SelectedProduct != null && SelectedProduct.Status != TranType.OnRepair);

            RepairProductCommand = new DelegateCommand(RepairProduct, () => SelectedProduct != null && (SelectedProduct.IDs.Count == 1) && (SelectedProduct.Status == TranType.Bought || SelectedProduct.Status == TranType.Repaired));
            CloseRepairDialogCommand = new DelegateCommand(() => RepairDialogViewModel = null);

            RepairedProductCommand = new DelegateCommand(RepairedProduct, () => SelectedProduct != null && (SelectedProduct.IDs.Count == 1) && SelectedProduct.Status == TranType.OnRepair);
            CloseRepairedDialogCommand = new DelegateCommand(() => RepairedDialogViewModel = null);
            
            EditProductCommand = new DelegateCommand(EditProduct, () => SelectedProduct != null);
            
            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => SelectedProduct != null);
            
            OnAirProductCommand = new DelegateCommand(OnAirProduct, () => SelectedProduct != null && (SelectedProduct.Status == TranType.Bought || SelectedProduct.Status == TranType.Repaired));
            CloseOnAirDialogCommand = new DelegateCommand(() => OnAirProductDialogViewModel = null);

            FromAirCommand = new DelegateCommand(FromAirProduct, () => SelectedProduct != null && SelectedProduct.Status == TranType.OnAir);
            CloseFromAirDialogCommand = new DelegateCommand(() => FromAirProductDialogViewModel = null);

            WriteOffProductCommand = new DelegateCommand(WriteOffProduct, () => SelectedProduct != null);

            ClearSerialNumberFilterCommand = new DelegateCommand(() => SerialNumberFilter = "");
            ClearModelFilterCommand = new DelegateCommand(() => ModelFilter = "");

            FromDateFilter = new DateTime(2015, 9, 1);
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

                if (value != null) SelectedProductIDs = value.IDs;
                
                SellProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
                DeleteProductCommand.RaiseCanExecuteChanged();
                RepairProductCommand.RaiseCanExecuteChanged();
                RepairedProductCommand.RaiseCanExecuteChanged();
                OnAirProductCommand.RaiseCanExecuteChanged();
                FromAirCommand.RaiseCanExecuteChanged();
                WriteOffProductCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _showOnAir;

        public bool ShowOnAir
        {
            get { return _showOnAir; }
            set
            {
                _showOnAir = value;
                NotifyPropertyChanged();
            }
        }

        #region Delegate commands

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

        private DelegateCommand _closeOnAirDialogCommand;

        public DelegateCommand CloseOnAirDialogCommand
        {
            get { return _closeOnAirDialogCommand; }
            set
            {
                _closeOnAirDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closeFromAirDialogCommand;

        public DelegateCommand CloseFromAirDialogCommand
        {
            get { return _closeFromAirDialogCommand; }
            set
            {
                _closeFromAirDialogCommand = value;
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

        private DelegateCommand _repairedProductCommand;

        public DelegateCommand RepairedProductCommand
        {
            get { return _repairedProductCommand; }
            set
            {
                _repairedProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _onAirProductCommand;

        public DelegateCommand OnAirProductCommand
        {
            get { return _onAirProductCommand; }
            set
            {
                _onAirProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _fromAirCommand;

        public DelegateCommand FromAirCommand
        {
            get { return _fromAirCommand; }
            set
            {
                _fromAirCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _writeOffProductCommand;

        public DelegateCommand WriteOffProductCommand
        {
            get { return _writeOffProductCommand; }
            set
            {
                _writeOffProductCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _clearSerialNumberFilterCommand;

        public DelegateCommand ClearSerialNumberFilterCommand
        {
            get { return _clearSerialNumberFilterCommand; }
            set
            {
                _clearSerialNumberFilterCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _clearModelFilterCommand;

        public DelegateCommand ClearModelFilterCommand
        {
            get { return _clearModelFilterCommand; }
            set
            {
                _clearModelFilterCommand = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Dialog view models
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

        private RepairOnStockProductDialogViewModel _repairDialogViewModel;

        public RepairOnStockProductDialogViewModel RepairDialogViewModel
        {
            get { return _repairDialogViewModel; }
            set
            {
                _repairDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private RepairedOnStockProductDialogViewModel _repairedDialogViewModel;

        public RepairedOnStockProductDialogViewModel RepairedDialogViewModel
        {
            get { return _repairedDialogViewModel; }
            set
            {
                _repairedDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private OnAirProductDialogViewModel _onAirProductDialogViewModel;

        public OnAirProductDialogViewModel OnAirProductDialogViewModel
        {
            get { return _onAirProductDialogViewModel; }
            set
            {
                _onAirProductDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private FromAirProductDialogViewModel _fromAirProductDialogViewModel;

        public FromAirProductDialogViewModel FromAirProductDialogViewModel
        {
            get { return _fromAirProductDialogViewModel; }
            set
            {
                _fromAirProductDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Methods
        public void AddProduct()
        {
            AddDialogViewModel = new AddOnStockProductDialogViewModel(_mainViewModel);
        }

        public void RepairProduct()
        {
            RepairDialogViewModel = new RepairOnStockProductDialogViewModel(_mainViewModel);
        }

        /// <summary>
        /// Add new transaction with SelectedProduct and TypeID "Repaired"
        /// </summary>
        public void RepairedProduct()
        {
            RepairedDialogViewModel = new RepairedOnStockProductDialogViewModel(_mainViewModel);
        }

        private void OnAirProduct()
        {
            OnAirProductDialogViewModel = new OnAirProductDialogViewModel(_mainViewModel);
        }

        private void FromAirProduct()
        {
            FromAirProductDialogViewModel = new FromAirProductDialogViewModel(_mainViewModel);
        }

        private void WriteOffProduct()
        {
            if (SelectedProduct == null) return;

            MessageBoxResult answer = MessageBox.Show("Списать выбранный товар?", "Question", MessageBoxButton.YesNo);

            if (answer != MessageBoxResult.Yes) return;

            foreach (var id in SelectedProduct.IDs)
            {
                Transaction transaction = new Transaction
                {
                    ProductID = id,
                    Date = DateTime.Now,
                    TypeID = (int) TranType.Sold,
                    OperatorID = OperatorManager.Instance.CurrentUserID,
                    Price = 0
                };
                _mainViewModel.Context.Transactions.Add(transaction);
            }

            try
            {
                var productToWriteOff =
                    Products.FirstOrDefault(x => x.IDs.Contains(SelectedProduct.IDs.FirstOrDefault()));
                if (productToWriteOff != null) _mainViewModel.ProductsValue -= productToWriteOff.MoneySpentForProduct*SelectedProduct.IDs.Count;
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось списать товар"), "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed write off product. " + e.InnerException.Message);
            }

            Update();
            _mainViewModel.SoldProductsViewModel.Update();
        }
        #endregion

        protected override bool Filter(object obj)
        {
            if (!base.Filter(obj)) return false;

            var product = obj as OnStockProductViewModel;
            if (product == null) return false;

            if ((product.Status == TranType.OnAir) != ShowOnAir) return false;
            if (SelectedCategoryFilter.Name != "Any" && product.CategoryName != SelectedCategoryFilter.Name) return false;

            return product.DateBought >= FromDateFilter && product.DateBought <= ThroughDateFilter;
        }

        protected override List<ProductViewModel> GetProducts(Context context)
        {
            var notSoldProducts =
                context.Products.Where(x => x.Transactions.Any()).ToList()
                    .Where(x => x.Transactions.OrderBy(y => y.Date).Last().TypeID != (int) TranType.Sold);

            return
                notSoldProducts.GroupBy(x => new ModelSerialNumberStatusPriceBoughtGroup(x))
                    .Select(x => new OnStockProductViewModel(x))
                      .Where(y => y.Origin == TranType.Bought)
                    .Cast<ProductViewModel>()
                    .ToList();


            //return
            //    notSoldProducts.Select(x => new OnStockProductViewModel(x))
            //        .Where(y => y.Origin == TranType.Bought)
            //        .Cast<ProductViewModel>()
            //        .ToList();
        }
    }
}
