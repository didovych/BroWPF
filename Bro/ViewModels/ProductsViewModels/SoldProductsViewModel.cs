using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows;
using Bro.ViewModels.ProductsViewModels;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels
{
    public class SoldProductsViewModel : ProductsViewModel
    {
        public SoldProductsViewModel(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => SelectedProduct != null);

            ClearSerialNumberFilterCommand = new DelegateCommand(() => SerialNumberFilter = "");
            ClearModelFilterCommand = new DelegateCommand(() => ModelFilter = "");
        }

        private SoldProductViewModel _selectedProduct;

        public SoldProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                if (value != null) SelectedProductIDs = value.IDs;
                NotifyPropertyChanged();
                DeleteProductCommand.RaiseCanExecuteChanged();
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

        protected override List<ProductViewModel> GetProducts(Context context)
        {
            var products =
                context.Products.Where(x => x.Transactions.Any()).ToList()
                    .Where(x => x.Transactions.OrderBy(y => y.Date).Last().TypeID == (int)TranType.Sold);

            return products.Select(x => new SoldProductViewModel(x)).Cast<ProductViewModel>().ToList();
        }

        protected override bool Filter(object obj)
        {
            if (!base.Filter(obj)) return false;

            var product = obj as SoldProductViewModel;
            if (product == null) return false;

            if (SelectedCategoryFilter.Name != "Any" && product.CategoryName != SelectedCategoryFilter.Name) return false;
            if (SelectedSalesmanFilter.Contragent.LastName != "Any" &&
                product.SalesmanSold.LastName != SelectedSalesmanFilter.Contragent.LastName) return false;
            if (SelectedOriginFilter != TranType.Any && product.Origin != SelectedOriginFilter) return false;

            return product.DateSold >= FromDateFilter && product.DateSold <= ThroughDateFilter;
        }
    }
}
