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
    public class RepairedToRepairProductDialogViewModel : ViewModelBase
    {
        public RepairedToRepairProductDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _repairedProduct = mainViewModel.ToRepairProductsViewModel.SelectedProduct;

            RepairedProductCommand = new DelegateCommand(RepairedProduct, Validate);
        }

        private readonly MainViewModel _mainViewModel;
        private readonly ToRepairProductViewModel _repairedProduct;

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyPropertyChanged();
                RepairedProductCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand RepairedProductCommand { get; set; }

        private void RepairedProduct()
        {
            if (_repairedProduct == null)
            {
                MessageBox.Show(("Не удалось вернуть товар с ремонта. Товар не был выбран."), "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed to take ToRepair product from repairer. Product was not selected.");
                _mainViewModel.ToRepairProductsViewModel.RepairedDialogViewModel = null;
            }

            int? lastRepairerID = null;
            try
            {
                lastRepairerID =
                _mainViewModel.Context.Transactions.OrderBy(x => x.Date).ToList().LastOrDefault(x => x.ProductID == _repairedProduct.IDs.FirstOrDefault() && x.TypeID == (int) TranType.OnRepair).ContragentID;
            }
            catch (Exception e)
            {
                Logging.WriteToLog("Failed take lastRepairerID");
            }

            Transaction transaction = new Transaction { ProductID = _repairedProduct.IDs.FirstOrDefault(), Date = DateTime.Now, TypeID = (int)TranType.Repaired, ContragentID = lastRepairerID, OperatorID = OperatorManager.Instance.CurrentUserID, Price = Price };

            try
            {
                _mainViewModel.Context.Transactions.Add(transaction);
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(("Не удалось вернуть товар с ремонта."), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Logging.WriteToLog("Failed add to DB take ToRepair product from repairer" + exception.Message);
            }

            _mainViewModel.ToRepairProductsViewModel.Update();
            _mainViewModel.CashInHand -= Price;
            _mainViewModel.ProductsValue += Price;

            _mainViewModel.ToRepairProductsViewModel.RepairedDialogViewModel = null;
        }

        private bool Validate()
        {
            return (Price >= 0);
        }
    }
}
