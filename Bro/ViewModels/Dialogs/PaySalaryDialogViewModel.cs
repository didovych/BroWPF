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
    public class PaySalaryDialogViewModel : ViewModelBase
    {
        public PaySalaryDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            PaySalaryCommand = new DelegateCommand(PaySalary, Validate);

            Employees = _mainViewModel.Context.Salesmen.Select(x => x.Contragent).ToList();
            Employees.AddRange(_mainViewModel.Context.Guards.Select(x => x.Contragent).ToList());
            SelectedEmployee = Employees.FirstOrDefault();
        }

        private readonly MainViewModel _mainViewModel;

        private List<Contragent> _employees;

        public List<Contragent> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                NotifyPropertyChanged();
                PaySalaryCommand.RaiseCanExecuteChanged();
            }
        }

        private Contragent _selectedEmployee;

        public Contragent SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                NotifyPropertyChanged();
                PaySalaryCommand.RaiseCanExecuteChanged();
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
                PaySalaryCommand.RaiseCanExecuteChanged();
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
                PaySalaryCommand.RaiseCanExecuteChanged();
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

        private bool Validate()
        {
            if (Amount <= 0) return false;
            if (SelectedEmployee == null) return false;

            return true;
        }

        private void PaySalary()
        {
            if (SelectedEmployee == null) _mainViewModel.CashTransactionsViewModel.PaySalaryViewModel = null;

            //TODO change operatorID
            var transaction = new Transaction {Date = DateTime.Now, Price = Amount, TypeID = (int) TranType.Salary, ContragentID = SelectedEmployee.ID, OperatorID = 1, Notes = Notes};

            _mainViewModel.Context.Transactions.Add(transaction);
            try
            {
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось провести проводку"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new salary transaction. " + e.Message);
            }

            _mainViewModel.CashTransactionsViewModel.Update();
            _mainViewModel.CashInHand -= Amount;
            _mainViewModel.CashTransactionsViewModel.PaySalaryViewModel = null;
        }
    }
}
