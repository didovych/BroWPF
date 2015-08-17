using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.ProductsViewModels
{
    public class ToPawnProductsViewModel : ProductsViewModel
    {
        public ToPawnProductsViewModel(Context context) : base(context)
        {
            AddProductCommand = new DelegateCommand(AddProduct);
            SellProductCommand = new DelegateCommand(SellProduct, () => SelectedProduct != null);
            EditProductCommand = new DelegateCommand(EditProduct, () => SelectedProduct != null);
            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => SelectedProduct != null);
            SendToOnStockProductCommand = new DelegateCommand(SendToOnStockProduct, () => SelectedProduct != null);
        }

        private ToPawnProductViewModel _selectedProduct;

        public ToPawnProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
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

        /// <summary>
        /// Create new transaction with TransactionType "ToPawn" and add it
        /// </summary>
        public void AddProduct()
        {
            MessageBox.Show("New product was added!");
        }

        public void SellProduct()
        {
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
        /// Edit the last "Bought" transaction with SelectedProduct, but disable to edit TransactionType (Onstock product must stay onstock)
        /// </summary>
        public void EditProduct()
        {
            MessageBox.Show("Selected product was edited!");
        }

        /// <summary>
        /// Add new transaction with Selected product and TransactionType "ToBought"
        /// </summary>
        public void SendToOnStockProduct()
        {
            MessageBox.Show("Selected product was sent to OnStock!");
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
    }
}
