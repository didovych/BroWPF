using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Bro.ViewModels.Dialogs;
using BroData;
using Microsoft.Practices.Prism.Commands;
using MessageBox = System.Windows.MessageBox;

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
            CloseSellDialogCommand = new DelegateCommand(() => SellDialogViewModel = null);

            RepairProductCommand = new DelegateCommand(RepairProduct, () => SelectedProduct != null && (SelectedProduct.Status == TranType.Bought || SelectedProduct.Status == TranType.Repaired));
            CloseRepairDialogCommand = new DelegateCommand(() => RepairDialogViewModel = null);

            RepairedProductCommand = new DelegateCommand(RepairedProduct, () => SelectedProduct != null && SelectedProduct.Status == TranType.OnRepair);
            CloseRepairedDialogCommand = new DelegateCommand(() => RepairedDialogViewModel = null);
            
            EditProductCommand = new DelegateCommand(EditProduct, () => SelectedProduct != null);

            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => SelectedProduct != null);
            
            OnAirProductCommand = new DelegateCommand(OnAirProduct, () => SelectedProduct != null && (SelectedProduct.Status == TranType.Bought || SelectedProduct.Status == TranType.Repaired));
            FromAirCommand = new DelegateCommand(FromAirProduct, () => SelectedProduct != null && SelectedProduct.Status == TranType.OnAir);
        }

        private readonly MainViewModel _mainViewModel;

        protected override int SelectedProductID { get; set; }

        private OnStockProductViewModel _selectedProduct;

        public OnStockProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyPropertyChanged();
                SelectedProductID = value.ID;
                SellProductCommand.RaiseCanExecuteChanged();
                EditProductCommand.RaiseCanExecuteChanged();
                DeleteProductCommand.RaiseCanExecuteChanged();
                RepairProductCommand.RaiseCanExecuteChanged();
                RepairedProductCommand.RaiseCanExecuteChanged();
                OnAirProductCommand.RaiseCanExecuteChanged();
                FromAirCommand.RaiseCanExecuteChanged();
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

        private DelegateCommand _closeSellDialogCommand;

        public DelegateCommand CloseSellDialogCommand
        {
            get { return _closeSellDialogCommand; }
            set
            {
                _closeSellDialogCommand = value;
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

        private SellOnStockProductDialogViewModel _sellDialogViewModel;

        public SellOnStockProductDialogViewModel SellDialogViewModel
        {
            get { return _sellDialogViewModel; }
            set
            {
                _sellDialogViewModel = value;
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
        #endregion

        #region Methods
        public void AddProduct()
        {
            AddDialogViewModel = new AddOnStockProductDialogViewModel(_mainViewModel);
        }

        public void SellProduct()
        {
            SellDialogViewModel = new SellOnStockProductDialogViewModel(_mainViewModel);
        }

        /// <summary>
        /// Edit the last transaction with SelectedProduct, but disable to edit TransactionType (Onstock product must stay onstock)
        /// </summary>
        public void EditProduct()
        {
            MessageBox.Show("Selected product was edited!");
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
            //TODO change operatorID
            Transaction transaction = new Transaction {ProductID = SelectedProduct.ID, Date = DateTime.Now, TypeID = (int) TranType.OnAir, OperatorID = 1};

            try
            {
                _mainViewModel.Context.Transactions.Add(transaction);
                _mainViewModel.Context.SaveChanges();

                MessageBox.Show(("Товар передан на выносную торговлю"), "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось перевести товар на выносную"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new  onAir transaction" + e.Message);
            }
        }

        private void FromAirProduct()
        {
            //TODO change operatorID
            Transaction transaction = new Transaction{ProductID = SelectedProduct.ID, Date = DateTime.Now, TypeID = (int) TranType.Bought, OperatorID = 1, Price = 0};

            try
            {
                _mainViewModel.Context.Transactions.Add(transaction);
                _mainViewModel.Context.SaveChanges();

                MessageBox.Show(("Товар переведен на приход"), "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось забрать товар с выносной"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new  onAir -> Bought transaction" + e.Message);
            }
        }
        #endregion

        

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
