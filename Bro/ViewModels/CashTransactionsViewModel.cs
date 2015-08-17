using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels
{
    public class CashTransactionsViewModel : ViewModelBase
    {
        public CashTransactionsViewModel(Context context)
        {
            CashTransactions = new ObservableCollection<CashTransactionViewModel>(GetCashTransactions(context));
            CashTransactionsView = CollectionViewSource.GetDefaultView(CashTransactions);

            AddCashTransactionCommand = new DelegateCommand(AddCashTransaction);
            EditCashTransactionCommand = new DelegateCommand(EditCashTransaction, () => SelectedCashTransaction != null);
            DeleteCashTransactionCommand = new DelegateCommand(DeleteCashTransaction, () => SelectedCashTransaction != null);
        }

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
                EditCashTransactionCommand.RaiseCanExecuteChanged();
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

        private DelegateCommand _editCashTransactionCommand;

        public DelegateCommand EditCashTransactionCommand
        {
            get { return _editCashTransactionCommand; }
            set
            {
                _editCashTransactionCommand = value;
                NotifyPropertyChanged();
            }
        }

        public void AddCashTransaction()
        {
            MessageBox.Show("New cashtransaction was added");
        }

        public void EditCashTransaction()
        {
            MessageBox.Show(String.Format("Cash transaction {0} was edited", SelectedCashTransaction.ID));
        }

        public void DeleteCashTransaction()
        {
            MessageBox.Show(String.Format("Cash transaction {0} was deleted", SelectedCashTransaction.ID));
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
                    x => x.TransactionType.ID == (int) TranType.Cashin || x.TransactionType.ID == (int) TranType.Cashout).ToList()
                    .Select(x => new CashTransactionViewModel(x)).ToList();
        }
    }
}
