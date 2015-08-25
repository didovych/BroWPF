using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows;
using Bro.ViewModels.ProductsViewModels;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class RepairOnStockProductDialogViewModel : ViewModelBase
    {
        public RepairOnStockProductDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _productToRepair = mainViewModel.OnStockProductsViewModel.SelectedProduct;

            RepairProductCommand = new DelegateCommand(RepairProduct, Validate);

            Repairers = mainViewModel.Context.Repairers.ToList();
            SelectedRepairer = Repairers.FirstOrDefault();
        }

        private readonly MainViewModel _mainViewModel;

        private readonly OnStockProductViewModel _productToRepair;

        private List<Repairer> _repairers;

        public List<Repairer> Repairers
        {
            get { return _repairers; }
            set
            {
                _repairers = value;
                NotifyPropertyChanged();
            }
        }

        private Repairer _selectedRepairer;

        public Repairer SelectedRepairer
        {
            get { return _selectedRepairer; }
            set
            {
                _selectedRepairer = value;
                NotifyPropertyChanged();
                RepairProductCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand RepairProductCommand { get; set; }

        private void RepairProduct()
        {
            if (_productToRepair == null)
            {
                MessageBox.Show(("Не удалось отправить товар на ремонт. Товар не был выбран."), "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed to send OnStock product to repair. Product was not selected.");
                _mainViewModel.OnStockProductsViewModel.RepairDialogViewModel = null;
            }

            List<Transaction> transactions = new List<Transaction>();

            foreach (var id in _productToRepair.IDs)
            {
                // TODO change operatorID
                transactions.Add(new Transaction
                {
                    ProductID = id,
                    Date = DateTime.Now,
                    TypeID = (int) TranType.OnRepair,
                    ContragentID = SelectedRepairer.ID,
                    OperatorID = 1,
                    Price = 0
                });
            }

            try
            {
                _mainViewModel.Context.Transactions.AddRange(transactions);
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(("Не удалось отправить товар на ремонт."), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Logging.WriteToLog("Failed to add to DB send OnStock product to repair" + exception.Message);
            }

            _mainViewModel.OnStockProductsViewModel.Update();

            _mainViewModel.OnStockProductsViewModel.RepairDialogViewModel = null;
        }

        private bool Validate()
        {
            return SelectedRepairer != null;
        }
    }
}
