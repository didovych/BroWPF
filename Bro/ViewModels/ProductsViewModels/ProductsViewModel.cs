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
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels
{
    public abstract class ProductsViewModel : ViewModelBase
    {
        protected ProductsViewModel(Context context)
        {
            Context = context;

            Products = new ObservableCollection<ProductViewModel>(GetProducts(context));
            ProductsView = CollectionViewSource.GetDefaultView(Products);
        }

        protected Context Context { get; set; }

        protected abstract int SelectedProductID { get; set; }

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
        }

        protected abstract List<ProductViewModel> GetProducts(Context context);
    }
}
