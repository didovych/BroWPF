using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;
using BroData;

namespace Bro.ViewModels
{
    public abstract class ProductsViewModel : ViewModelBase
    {
        protected ProductsViewModel(Context context)
        {
            Products = new ObservableCollection<ProductViewModel>(GetProducts(context));
            ProductsView = CollectionViewSource.GetDefaultView(Products);

            Context = context;
        }

        protected Context Context { get; set; }

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

        protected abstract List<ProductViewModel> GetProducts(Context context);
    }
}
