using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows;
using Bro.ViewModels.Dialogs;
using BroData;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels
{
    public abstract class ProductsViewModel : ViewModelBase
    {
        protected ProductsViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            FromDateFilter = DateTime.Today.AddDays(-14);
            ThroughDateFilter = DateTime.Today.AddDays(1);

            CategoryFilter = mainViewModel.Context.Categories.ToList();
            CategoryFilter.Insert(0, new Category{Name = "Any"});
            SelectedCategoryFilter = CategoryFilter.FirstOrDefault();

            SalesmanFilter = mainViewModel.Context.Salesmen.ToList();
            SalesmanFilter.Insert(0, new Salesman{Contragent = new Contragent{LastName = "Any"}});
            SelectedSalesmanFilter = SalesmanFilter.FirstOrDefault();

            OriginFilter = new List<TranType>{TranType.Any, TranType.Bought, TranType.ToRepair, TranType.ToPawn};
            SelectedOriginFilter = OriginFilter.FirstOrDefault();

            Products = new ObservableCollection<ProductViewModel>(GetProducts(MainViewModel.Context));
            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = Filter;
            
            CloseSellProductDialog = new DelegateCommand(() => SellProductDialogViewModel = null);
            CloseEditProductDialogCommand = new DelegateCommand(() => EditProductDialogViewModel = null);

            ShowAllTransactionsCommand = new DelegateCommand(ShowAllTransactions);
            CloseShowAllTransactionsDialogCommand = new DelegateCommand(() => ShowAllTransactionsDialogViewModel = null);

            FilterCommand = new DelegateCommand(ProductsView.Refresh);
        }

        protected MainViewModel MainViewModel { get; set; }

        protected List<int> SelectedProductIDs { get; set; }

        #region Delegate commands
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

        private DelegateCommand _closeSellProductDialog;

        public DelegateCommand CloseSellProductDialog
        {
            get { return _closeSellProductDialog; }
            set
            {
                _closeSellProductDialog = value;
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

        private DelegateCommand _showAllTransactionsCommand;

        public DelegateCommand ShowAllTransactionsCommand
        {
            get { return _showAllTransactionsCommand; }
            set
            {
                _showAllTransactionsCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closeShowAllTransactionsDialogCommand;

        public DelegateCommand CloseShowAllTransactionsDialogCommand
        {
            get { return _closeShowAllTransactionsDialogCommand; }
            set
            {
                _closeShowAllTransactionsDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _filterCommand;

        public DelegateCommand FilterCommand
        {
            get { return _filterCommand; }
            set
            {
                _filterCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closeEditProductDialogCommand;

        public DelegateCommand CloseEditProductDialogCommand
        {
            get { return _closeEditProductDialogCommand; }
            set
            {
                _closeEditProductDialogCommand = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        private SellProductDialogViewModel _sellProductDialogViewModel;

        public SellProductDialogViewModel SellProductDialogViewModel
        {
            get { return _sellProductDialogViewModel; }
            set
            {
                _sellProductDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private EditProductDialogViewModel _editProductDialogViewModel;

        public EditProductDialogViewModel EditProductDialogViewModel
        {
            get { return _editProductDialogViewModel; }
            set
            {
                _editProductDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private ShowAllTransactionsDialogViewModel _showAllTransactionsDialogViewModel;

        public ShowAllTransactionsDialogViewModel ShowAllTransactionsDialogViewModel
        {
            get { return _showAllTransactionsDialogViewModel; }
            set
            {
                _showAllTransactionsDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        #region products
        private ObservableCollection<ProductViewModel> _products;

        public ObservableCollection<ProductViewModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyPropertyChanged();
            }
        }

        private ICollectionView _productsView;

        public ICollectionView ProductsView
        {
            get { return _productsView; }
            set
            {
                _productsView = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

#region Filters
        private DateTime _fromDateFilter;

        public DateTime FromDateFilter
        {
            get { return _fromDateFilter; }
            set
            {
                _fromDateFilter = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _throughDateFilter;

        public DateTime ThroughDateFilter
        {
            get { return _throughDateFilter; }
            set
            {
                _throughDateFilter = value;
                NotifyPropertyChanged();
            }
        }

        private List<Category> _categoryFilter;

        public List<Category> CategoryFilter
        {
            get { return _categoryFilter; }
            set
            {
                _categoryFilter = value;
                NotifyPropertyChanged();
            }
        }

        private Category _selectedCategoryFilter;

        public Category SelectedCategoryFilter
        {
            get { return _selectedCategoryFilter; }
            set
            {
                _selectedCategoryFilter = value;
                NotifyPropertyChanged();
            }
        }

        private List<Salesman> _salesmanFilter;

        public List<Salesman> SalesmanFilter
        {
            get { return _salesmanFilter; }
            set
            {
                _salesmanFilter = value;
                NotifyPropertyChanged();
            }
        }

        private Salesman _selectedSalesmanFilter;

        public Salesman SelectedSalesmanFilter
        {
            get { return _selectedSalesmanFilter; }
            set
            {
                _selectedSalesmanFilter = value;
                NotifyPropertyChanged();
            }
        }

        private List<TranType> _originFilter;

        public List<TranType> OriginFilter
        {
            get { return _originFilter; }
            set
            {
                _originFilter = value;
                NotifyPropertyChanged();
            }
        }

        private TranType _selectedOriginFilter;

        public TranType SelectedOriginFilter
        {
            get { return _selectedOriginFilter; }
            set
            {
                _selectedOriginFilter = value;
                NotifyPropertyChanged();
            }
        }

        private string _serialNumberFilter;

        public string SerialNumberFilter
        {
            get { return _serialNumberFilter; }
            set
            {
                _serialNumberFilter = value;
                NotifyPropertyChanged();
                ProductsView.Refresh();
            }
        }

        private string _modelFilter;

        public string ModelFilter
        {
            get { return _modelFilter; }
            set
            {
                _modelFilter = value;
                NotifyPropertyChanged();
                ProductsView.Refresh();
            }
        }

#endregion

        public void Update()
        {
            Products.Clear();
            Products.AddRange(GetProducts(MainViewModel.Context));
        }

        /// <summary>
        /// Find all transactions with SelectedProduct and delete all of them
        /// </summary>
        protected void DeleteProduct()
        {
            MessageBoxResult answer = MessageBox.Show("Удалить выбранный товар?", "Question", MessageBoxButton.YesNo);

            if (answer != MessageBoxResult.Yes) return;

            var transactionsToDelete =
                MainViewModel.Context.Transactions.Where(x => x.ProductID != null && SelectedProductIDs.Contains(x.ProductID.Value)).ToList();

            foreach (var transaction in transactionsToDelete)
            {
                MainViewModel.Context.Transactions.Remove(transaction);
            }

            var productsToDelete = MainViewModel.Context.Products.ToList()
                .Where(x => SelectedProductIDs.Contains(x.ID)).ToList();

            foreach (var productToDelete in productsToDelete)
            {
                MainViewModel.Context.Products.Remove(productToDelete);
            }

            try
            {
                MainViewModel.Context.SaveChanges();

                //Update CashInHand and Products value
                MainViewModel.UpdateCashInHand();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось удалить товар"), "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed delete product. " + e.InnerException.Message);
            }

            MessageBox.Show("Товар удален", "Confirmation", MessageBoxButton.OK);

            Update();
        }

        protected void EditProduct()
        {
            if (SelectedProductIDs == null) return;
            EditProductDialogViewModel = new EditProductDialogViewModel(MainViewModel.Context, SelectedProductIDs, this);
        }

        protected void SellProduct()
        {
            if (SelectedProductIDs == null) return;
            SellProductDialogViewModel = new SellProductDialogViewModel(MainViewModel, SelectedProductIDs, this);
        }

        protected void ShowAllTransactions()
        {
            if (SelectedProductIDs == null) return;
            ShowAllTransactionsDialogViewModel = new ShowAllTransactionsDialogViewModel(MainViewModel.Context, SelectedProductIDs, this);
        }

        protected abstract List<ProductViewModel> GetProducts(Context context);

        protected virtual bool Filter(object obj)
        {
            var product = obj as ProductViewModel;

            if (product == null) return false;
            if (!string.IsNullOrEmpty(ModelFilter) &&
                (product.ModelName == null || !product.ModelName.ToLower().Contains(ModelFilter.ToLower())))
                return false;
            return (string.IsNullOrEmpty(SerialNumberFilter) || (product.SerialNumber != null && product.SerialNumber.ToLower().Contains(SerialNumberFilter)));
        }
    }
}
