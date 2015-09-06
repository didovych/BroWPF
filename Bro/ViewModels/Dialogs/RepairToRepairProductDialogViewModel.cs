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
    public class RepairToRepairProductDialogViewModel : ViewModelBase
    {
        public RepairToRepairProductDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _productToRepair = mainViewModel.ToRepairProductsViewModel.SelectedProduct;

            RepairProductCommand = new DelegateCommand(RepairProduct, Validate);

            Repairers = mainViewModel.Context.Repairers.ToList();
            SelectedRepairer = Repairers.FirstOrDefault();
        }

        private readonly MainViewModel _mainViewModel;

        private readonly ToRepairProductViewModel _productToRepair;

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
                _mainViewModel.ToRepairProductsViewModel.RepairDialogViewModel = null;
            }

            Transaction transaction = new Transaction {ProductID = _productToRepair.IDs.FirstOrDefault(), Date = DateTime.Now, TypeID = (int) TranType.OnRepair, ContragentID = SelectedRepairer.ID, OperatorID = OperatorManager.Instance.CurrentUserID, Price = 0};

            try
            {
                _mainViewModel.Context.Transactions.Add(transaction);
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(("Не удалось отправить товар на ремонт."), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Logging.WriteToLog("Failed to add to DB send ToRepair product to repair" + exception.Message);
            }

            _mainViewModel.ToRepairProductsViewModel.Update();

            _mainViewModel.ToRepairProductsViewModel.RepairDialogViewModel = null;
        }

        private bool Validate()
        {
            return SelectedRepairer != null;
        }
    }
}
