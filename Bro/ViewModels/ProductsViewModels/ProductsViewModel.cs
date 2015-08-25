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
        protected ProductsViewModel(Context context)
        {
            Context = context;

            FromDateFilter = DateTime.Today.AddDays(-14);
            ThroughDateFilter = DateTime.Today.AddDays(1);

            Products = new ObservableCollection<ProductViewModel>(GetProducts(context));
            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = Filter;
            
            CloseSellProductDialog = new DelegateCommand(() => SellProductDialogViewModel = null);
            CloseEditProductDialogCommand = new DelegateCommand(() => EditProductDialogViewModel = null);

            FilterCommand = new DelegateCommand(ProductsView.Refresh);
        }

        protected Context Context { get; set; }

        protected int? SelectedProductID { get; set; }

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

        #endregion

        public void Update()
        {
            Products.Clear();
            Products.AddRange(GetProducts(Context));
        }

        /// <summary>
        /// Find all transactions with SelectedProduct and delete all of them
        /// </summary>
        protected void DeleteProduct()
        {
            MessageBoxResult answer = MessageBox.Show("Удалить выбранный товар?", "Question", MessageBoxButton.YesNo);

            if (answer != MessageBoxResult.Yes) return;

            var transactionsToDelete =
                Context.Transactions.Where(x => x.ProductID == SelectedProductID).ToList();

            foreach (var transaction in transactionsToDelete)
            {
                Context.Transactions.Remove(transaction);
            }

            var productToDelete = Context.Products.ToList()
                .LastOrDefault(x => x.ID == SelectedProductID);

            Context.Products.Remove(productToDelete);

            Context.SaveChanges();

            MessageBox.Show("Товар удален", "Confirmation", MessageBoxButton.OK);

            Update();
        }

        protected void EditProduct()
        {
            if (SelectedProductID != null)
                EditProductDialogViewModel = new EditProductDialogViewModel(Context, SelectedProductID.Value, this);
        }

        protected void SellProduct()
        {
            if (SelectedProductID != null)
                SellProductDialogViewModel = new SellProductDialogViewModel(Context, SelectedProductID.Value, this);
        }

        protected abstract List<ProductViewModel> GetProducts(Context context);
        protected abstract bool Filter(object obj);
    }
}
