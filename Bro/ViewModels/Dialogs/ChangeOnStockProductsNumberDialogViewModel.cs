using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bro.Services;
using Bro.ViewModels.ProductsViewModels;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class ChangeOnStockProductsNumberDialogViewModel: ViewModelBase
    {
        public ChangeOnStockProductsNumberDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _selectedProduct = mainViewModel.OnStockProductsViewModel.SelectedProduct;

            ChangeNumberCommand = new DelegateCommand(ChangeNumber, () => Validate());

            Number = _selectedProduct.IDs.Count;
        }

        private readonly MainViewModel _mainViewModel;
        private readonly OnStockProductViewModel _selectedProduct;

        private int _number;

        public int Number
        {
            get { return _number; }
            set
            {
                _number = value; 
                NotifyPropertyChanged();
                ChangeNumberCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ChangeNumberCommand { get; set; }

        private bool Validate()
        {
            return Number > 0;
        }

        private void ChangeNumber()
        {
            int selectedProductCount = _selectedProduct.IDs.Count;

            if (Number > selectedProductCount)
                for (int i = 0; i < Number - selectedProductCount; i++) AddProduct();
            else if (Number < selectedProductCount)
                for (int i = 0; i < selectedProductCount - Number; i++) DeleteProduct(_selectedProduct.IDs[i]);

            try
            {
                _mainViewModel.Context.SaveChanges();

                _mainViewModel.UpdateCashInHand();
                _mainViewModel.OnStockProductsViewModel.Update();
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось изменить количество товара", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logging.WriteToLog("Failed to change number of products. Number=" + Number + " Ids.Count=" + selectedProductCount + ". " + e.InnerException.Message);
            }

            _mainViewModel.OnStockProductsViewModel.ChangeNumberDialogViewModel = null;
        }

        private void AddProduct()
        {
            Product product = new Product
            {
                ModelID = _selectedProduct.ModelID,
                SerialNumber = _selectedProduct.SerialNumber,
                Notes = _selectedProduct.Notes,
                SellingPrice = _selectedProduct.SellingPrice,
                DateSellTo = _selectedProduct.DateSellTo
            };

            _mainViewModel.Context.Products.Add(product);

            Transaction transaction = new Transaction
            {
                Product = product,
                Date = DateTime.Now,
                TypeID = (int)TranType.Bought,
                OperatorID = OperatorManager.Instance.CurrentUserID,
                Price = _selectedProduct.Price
            };

            _mainViewModel.Context.Transactions.Add(transaction);
        }

        private void DeleteProduct(int id)
        {
            var transactionsToDelete =
                _mainViewModel.Context.Transactions.Where(x => x.ProductID != null && x.ProductID.Value == id);

            foreach (var transaction in transactionsToDelete)
            {
                _mainViewModel.Context.Transactions.Remove(transaction);
            }

            var productToDelete = _mainViewModel.Context.Products.FirstOrDefault(x => x.ID == id);

            _mainViewModel.Context.Products.Remove(productToDelete);
        }
    }
}
