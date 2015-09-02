using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Bro.Converters;
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

            FromDateFilter = DateTime.Today.AddDays(-14);
            ThroughDateFilter = DateTime.Today.AddDays(1);

            SalesmanFilter = mainViewModel.Context.Salesmen.ToList();
            SalesmanFilter.Insert(0, new Salesman { Contragent = new Contragent { LastName = "Any" } });
            SelectedSalesmanFilter = SalesmanFilter.FirstOrDefault();

            ContragentsFilter = mainViewModel.Context.Salesmen.Select(x => x.Contragent).ToList();
            ContragentsFilter.AddRange(mainViewModel.Context.Guards.Select(x => x.Contragent).ToList());
            ContragentsFilter.Insert(0, new Contragent{LastName = "Any"});
            SelectedContragentFilter = ContragentsFilter.FirstOrDefault();

            TypeFilter = new List<TranType> {TranType.Any, TranType.CashIn, TranType.CashOut, TranType.Coffee, TranType.Salary};
            SelectedTypeFilter = TypeFilter.FirstOrDefault();

            CashTransactions = new ObservableCollection<CashTransactionViewModel>(GetCashTransactions(_mainViewModel.Context));
            CashTransactionsView = CollectionViewSource.GetDefaultView(CashTransactions);
            CashTransactionsView.Filter = Filter;

            AddCashTransactionCommand = new DelegateCommand(AddCashTransaction);
            CloseAddDialogCommand = new DelegateCommand(() => AddCashViewModel = null);

            PaySalaryCommand = new DelegateCommand(PaySalary);
            ClosePaySalaryDialogCommand = new DelegateCommand(() => PaySalaryViewModel = null);

            DeleteCashTransactionCommand = new DelegateCommand(DeleteCashTransaction, () => SelectedCashTransaction != null);

            FilterCommand = new DelegateCommand(CashTransactionsView.Refresh);
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

        private decimal _viewedTransactionsSum;

        public decimal ViewedTransactionsSum
        {
            get { return _viewedTransactionsSum; }
            set
            {
                _viewedTransactionsSum = value;
                NotifyPropertyChanged();
            }
        }

        #region Filters
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

        private List<Salesman> _salesmanFilter;

        public List<Salesman> SalesmanFilter
        {
            get { return _salesmanFilter; }
            set
            {
                _salesmanFilter = value;
                NotifyPropertyChanged();
            }
        }

        private Salesman _selectedSalesmanFilter;

        public Salesman SelectedSalesmanFilter
        {
            get { return _selectedSalesmanFilter; }
            set
            {
                _selectedSalesmanFilter = value;
                NotifyPropertyChanged();
            }
        }

        private List<TranType> _typeFilter;

        public List<TranType> TypeFilter
        {
            get { return _typeFilter; }
            set
            {
                _typeFilter = value;
                NotifyPropertyChanged();
            }
        }

        private TranType _selectedTypeFilter;

        public TranType SelectedTypeFilter
        {
            get { return _selectedTypeFilter; }
            set
            {
                _selectedTypeFilter = value;
                NotifyPropertyChanged();
            }
        }

        private List<Contragent> _contragentsFilter;

        public List<Contragent> ContragentsFilter
        {
            get { return _contragentsFilter; }
            set
            {
                _contragentsFilter = value;
                NotifyPropertyChanged();
            }
        }

        private Contragent _selectedContragentFilter;

        public Contragent SelectedContragentFilter
        {
            get { return _selectedContragentFilter; }
            set
            {
                _selectedContragentFilter = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Delegate commands
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
        #endregion

        #region Dialog view models
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
        #endregion

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
            _mainViewModel.UpdateCashInHand();
        }

        #region Transactions
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
        #endregion

        private bool Filter(object obj)
        {
            var transaction = obj as CashTransactionViewModel;

            if (transaction == null) return false;
            if (SelectedSalesmanFilter.Contragent.LastName != "Any" &&
                (transaction.Salesman == null || transaction.Salesman.LastName != SelectedSalesmanFilter.Contragent.LastName)) return false;
            if (SelectedContragentFilter.LastName != "Any" &&
                (transaction.Contragent == null || transaction.Contragent.LastName != SelectedContragentFilter.LastName)) return false;
            if (SelectedTypeFilter != TranType.Any && transaction.CashTranType != SelectedTypeFilter) return false;

            if (transaction.Date < FromDateFilter || transaction.Date > ThroughDateFilter) return false;

            ViewedTransactionsSum += transaction.Price;
            return true;
        }

        private List<CashTransactionViewModel> GetCashTransactions(Context context)
        {
            return
                context.Transactions.Where(
                    x => x.TypeID == (int)TranType.CashIn || x.TypeID == (int)TranType.CashOut || x.TypeID == (int)TranType.Coffee || x.TypeID == (int)TranType.Salary).ToList()
                    .Select(x => new CashTransactionViewModel(x)).ToList();
        }
    }
}
