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

namespace Bro.ViewModels.MobileTransactions
{
    public class MobileTransactionsViewModel: ViewModelBase
    {
        public MobileTransactionsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            FromDateFilter = DateTime.Today.AddDays(-14);
            ThroughDateFilter = DateTime.Today.AddDays(1);

            SalesmanFilter = mainViewModel.Context.Salesmen.ToList();
            SalesmanFilter.Insert(0, new Salesman { Contragent = new Contragent { LastName = "Any" } });
            SelectedSalesmanFilter = SalesmanFilter.FirstOrDefault();

            ClientsFilter = mainViewModel.Context.Clients.ToList().Select(x => x.Contragent).ToList();
            ClientsFilter.Insert(0, new Contragent { LastName = "Any" });
            SelectedClientFilter = ClientsFilter.FirstOrDefault();

            MobileTransactions = new ObservableCollection<MobileTransactionViewModel>(GetMobileTransactions(mainViewModel.Context));
            MobileTransactionsView = CollectionViewSource.GetDefaultView(MobileTransactions);
            MobileTransactionsView.Filter = Filter;

            AddCommand = new DelegateCommand(Add);
            CloseAddDialogCommand = new DelegateCommand(() => AddDialogViewModel = null);

            DeleteCommand = new DelegateCommand(Delete, () => SelectedTransaction != null);

            FilterCommand = new DelegateCommand(MobileTransactionsView.Refresh);
        }

        private MainViewModel _mainViewModel;

        private MobileTransactionViewModel _selectedTransaction;

        public MobileTransactionViewModel SelectedTransaction
        {
            get { return _selectedTransaction; }
            set
            {
                _selectedTransaction = value;
                NotifyPropertyChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        private AddMobileTransactionDialogViewModel _addDialogViewModel;

        public AddMobileTransactionDialogViewModel AddDialogViewModel
        {
            get { return _addDialogViewModel; }
            set
            {
                _addDialogViewModel = value;
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

        private List<Contragent> _clientsFilter;

        public List<Contragent> ClientsFilter
        {
            get { return _clientsFilter; }
            set
            {
                _clientsFilter = value;
                NotifyPropertyChanged();
            }
        }

        private Contragent _selectedClientFilter;

        public Contragent SelectedClientFilter
        {
            get { return _selectedClientFilter; }
            set
            {
                _selectedClientFilter = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Delegate commands

        private DelegateCommand _addCommand;

        public DelegateCommand AddCommand
        {
            get { return _addCommand; }
            set
            {
                _addCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closeAddDialogCommand;

        public DelegateCommand CloseAddDialogCommand
        {
            get { return _closeAddDialogCommand; }
            set { _closeAddDialogCommand = value; }
        }

        private DelegateCommand _deleteCommand;

        public DelegateCommand DeleteCommand
        {
            get { return _deleteCommand; }
            set
            {
                _deleteCommand = value;
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

        #region Mobile transactions

        private ObservableCollection<MobileTransactionViewModel> _mobileTransactions;

        public ObservableCollection<MobileTransactionViewModel> MobileTransactions
        {
            get { return _mobileTransactions; }
            set
            {
                _mobileTransactions = value;
                NotifyPropertyChanged();
            }
        }

        private ICollectionView _mobileTransactionsView;

        public ICollectionView MobileTransactionsView
        {
            get { return _mobileTransactionsView; }
            set
            {
                _mobileTransactionsView = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        private void Add()
        {
            AddDialogViewModel = new AddMobileTransactionDialogViewModel(_mainViewModel);
        }

        private void Delete()
        {
            if (SelectedTransaction == null) return;

            MessageBoxResult answer = MessageBox.Show("Удалить выбранную транзакцию?", "Question", MessageBoxButton.YesNo);

            if (answer != MessageBoxResult.Yes) return;

            var transactionToDelete = _mainViewModel.Context.Transactions.ToList()
                .LastOrDefault(x => x.ID == SelectedTransaction.ID);

            try
            {
                _mainViewModel.Context.Transactions.Remove(transactionToDelete);
                _mainViewModel.Context.SaveChanges();

                MessageBox.Show("Транзакция удалена.", "Confirmation", MessageBoxButton.OK);
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось удалить транзакцию", "Error", MessageBoxButton.OK);
                Logging.WriteToLog("Failed to remove mobile transaction. " + e.Message);
            }

            Update();
        }

        public void Update()
        {
            MobileTransactions.Clear();
            MobileTransactions.AddRange(GetMobileTransactions(_mainViewModel.Context));
        }

        private bool Filter(object obj)
        {
            var transaction = obj as MobileTransactionViewModel;

            if (transaction == null) return false;
            if (SelectedSalesmanFilter.Contragent.LastName != "Any" &&
                (transaction.Salesman == null || transaction.Salesman.LastName != SelectedSalesmanFilter.Contragent.LastName)) return false;
            if (SelectedClientFilter.LastName != "Any" &&
                (transaction.Client == null || transaction.Client.LastName != SelectedClientFilter.LastName)) return false;

            return transaction.Date >= FromDateFilter && transaction.Date <= ThroughDateFilter;
        }

        private List<MobileTransactionViewModel> GetMobileTransactions(Context context)
        {
            return context.MobileTransactions.ToList().Select(x => new MobileTransactionViewModel(x)).ToList();
        }
    }
}
