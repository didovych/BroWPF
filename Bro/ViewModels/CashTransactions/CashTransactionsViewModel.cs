using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Bro.ViewModels.Dialogs;
using BroData;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels
{
    public class CashTransactionsViewModel : ViewModelBase
    {
        public CashTransactionsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            CashTransactions = new ObservableCollection<CashTransactionViewModel>(GetCashTransactions(_mainViewModel.Context));
            CashTransactionsView = CollectionViewSource.GetDefaultView(CashTransactions);

            AddCashTransactionCommand = new DelegateCommand(AddCashTransaction);
            CloseAddDialogCommand = new DelegateCommand(() => AddCashViewModel = null);

            PaySalaryCommand = new DelegateCommand(PaySalary);
            ClosePaySalaryDialogCommand = new DelegateCommand(() => PaySalaryViewModel = null);

            DeleteCashTransactionCommand = new DelegateCommand(DeleteCashTransaction, () => SelectedCashTransaction != null);
        }

        private readonly MainViewModel _mainViewModel;

        private CashTransactionViewModel _selectedCashTransaction;

        public CashTransactionViewModel SelectedCashTransaction
        {
            get { return _selectedCashTransaction; }
            set
            {
                _selectedCashTransaction = value;
                NotifyPropertyChanged();
                DeleteCashTransactionCommand.RaiseCanExecuteChanged();
                AddCashTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _deleteCashTransactionCommand;

        public DelegateCommand DeleteCashTransactionCommand
        {
            get { return _deleteCashTransactionCommand; }
            set
            {
                _deleteCashTransactionCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _addCashTransactionCommand;

        public DelegateCommand AddCashTransactionCommand
        {
            get { return _addCashTransactionCommand; }
            set
            {
                _addCashTransactionCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closeAddDialogCommand;

        public DelegateCommand CloseAddDialogCommand
        {
            get { return _closeAddDialogCommand; }
            set
            {
                _closeAddDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _paySalaryCommand;

        public DelegateCommand PaySalaryCommand
        {
            get { return _paySalaryCommand; }
            set
            {
                _paySalaryCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closePaySalaryDialogCommand;

        public DelegateCommand ClosePaySalaryDialogCommand
        {
            get { return _closePaySalaryDialogCommand; }
            set
            {
                _closePaySalaryDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private AddCashDialogViewModel _addCashViewModel;

        public AddCashDialogViewModel AddCashViewModel
        {
            get { return _addCashViewModel; }
            set
            {
                _addCashViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private PaySalaryDialogViewModel _paySalaryViewModel;

        public PaySalaryDialogViewModel PaySalaryViewModel
        {
            get { return _paySalaryViewModel; }
            set
            {
                _paySalaryViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public void Update()
        {
            CashTransactions.Clear();
            CashTransactions.AddRange(GetCashTransactions(_mainViewModel.Context));
        }

        public void AddCashTransaction()
        {
            AddCashViewModel = new AddCashDialogViewModel(_mainViewModel);
        }

        private void PaySalary()
        {
            PaySalaryViewModel = new PaySalaryDialogViewModel(_mainViewModel);
        }

        public void DeleteCashTransaction()
        {
            if (SelectedCashTransaction == null) return;

            MessageBoxResult answer = MessageBox.Show("Удалить выбранную транзакцию?", "Question", MessageBoxButton.YesNo);

            if (answer != MessageBoxResult.Yes) return;

            var transactionToDelete = _mainViewModel.Context.Transactions.ToList()
                .LastOrDefault(x => x.ID == SelectedCashTransaction.ID);

            try
            {
                _mainViewModel.Context.Transactions.Remove(transactionToDelete);
                _mainViewModel.Context.SaveChanges();

                MessageBox.Show("Транзакция удалена.", "Confirmation", MessageBoxButton.OK);
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось удалить транзакцию", "Error", MessageBoxButton.OK);
                Logging.WriteToLog("Failed to remove cash transaction. " + e.Message);
            }

            Update();
        }

        private ObservableCollection<CashTransactionViewModel> _cashTransactions;

        public ObservableCollection<CashTransactionViewModel> CashTransactions
        {
            get { return _cashTransactions; }
            set
            {
                _cashTransactions = value;
                NotifyPropertyChanged();
            }
        }

        private ICollectionView _cashTransactionsView;

        public ICollectionView CashTransactionsView
        {
            get { return _cashTransactionsView; }
            set
            {
                _cashTransactionsView = value;
                NotifyPropertyChanged();
            }
        }

        private List<CashTransactionViewModel> GetCashTransactions(Context context)
        {
            return
                context.Transactions.Where(
                    x => x.TransactionType.ID == (int) TranType.Cash || x.TransactionType.ID == (int) TranType.Salary).ToList()
                    .Select(x => new CashTransactionViewModel(x)).ToList();
        }
    }
}
