using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bro.Services;
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

            Types = new List<string>{"Взять деньги", "Положить деньги", "Кофе/чай"};
            SelectedType = Types.First();
        }

        private readonly MainViewModel _mainViewModel;

        private List<string> _types;

        public List<string> Types
        {
            get { return _types; }
            set
            {
                _types = value;
                NotifyPropertyChanged();
                AddCashCommand.RaiseCanExecuteChanged();
            }
        }

        private string _selectedType;

        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                NotifyPropertyChanged();
                AddCashCommand.RaiseCanExecuteChanged();
                if (value == "Кофе/чай") Amount = 4;
            }
        }

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
            if (Amount <= 0) return false;
            if (SelectedType != "Кофе/чай" && string.IsNullOrEmpty(Notes)) return false;

            return true;
        }

        private void AddCash()
        {
            int typeID = (int) TranType.Zero;
            switch (SelectedType)
            {
                case "Взять деньги":
                    typeID = (int) TranType.CashOut;
                    _mainViewModel.CashInHand -= Amount;
                    break;
                case "Положить деньги":
                    typeID = (int) TranType.CashIn;
                    _mainViewModel.CashInHand += Amount;
                    break;
                case "Кофе/чай":
                    typeID = (int) TranType.Coffee;
                    _mainViewModel.CashInHand += Amount;
                    break;
            }

            var transaction = new Transaction {Date = DateTime.Now, Price = Amount, TypeID = typeID, OperatorID = OperatorManager.Instance.CurrentUserID, Notes = Notes};

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
