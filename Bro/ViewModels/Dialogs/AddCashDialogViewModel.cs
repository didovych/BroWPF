using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class AddCashDialogViewModel: ViewModelBase
    {
        public AddCashDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddCashCommand = new DelegateCommand(AddCash, Validate);
        }

        private readonly MainViewModel _mainViewModel;

        private decimal _amount;

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                NotifyPropertyChanged();
                AddCashCommand.RaiseCanExecuteChanged();
            }
        }

        private string _notes;

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                NotifyPropertyChanged();
                AddCashCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _addCashCommand;

        public DelegateCommand AddCashCommand
        {
            get { return _addCashCommand; }
            set
            {
                _addCashCommand = value;
                NotifyPropertyChanged();
            }
        }

        private bool Validate()
        {
            if (Amount < 0) return false;
            if (string.IsNullOrEmpty(Notes)) return false;

            return true;
        }

        private void AddCash()
        {
            //TODO change operatorID
            var transaction = new Transaction {Date = DateTime.Now, Price = Amount, TypeID = (int) TranType.Cash, OperatorID = 1, Notes = Notes};

            _mainViewModel.Context.Transactions.Add(transaction);
            try
            {
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось провести проводку"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new cash transaction. " + e.Message);
            }

            _mainViewModel.CashTransactionsViewModel.Update();
            _mainViewModel.CashTransactionsViewModel.AddCashViewModel = null;
        }
    }
}
