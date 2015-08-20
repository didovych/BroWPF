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
    public class AddSalesmanDialogViewModel : ViewModelBase
    {
        public AddSalesmanDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddSalesmanCommand = new DelegateCommand(AddSalesman, Validate);
        }

        private readonly MainViewModel _mainViewModel;

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyPropertyChanged();
                AddSalesmanCommand.RaiseCanExecuteChanged();
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyPropertyChanged();
                AddSalesmanCommand.RaiseCanExecuteChanged();
            }
        }

        private int _profitPercentage;

        public int ProfitPercentage
        {
            get { return _profitPercentage; }
            set
            {
                _profitPercentage = value;
                NotifyPropertyChanged();
            }
        }

        private int _salaryPerDay;

        public int SalaryPerDay
        {
            get { return _salaryPerDay; }
            set
            {
                _salaryPerDay = value;
                NotifyPropertyChanged();
            }
        }

        public DelegateCommand AddSalesmanCommand { get; set; }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(FirstName)) return false;
            if (string.IsNullOrEmpty(LastName)) return false;

            return true;
        }

        private void AddSalesman()
        {
            FirstName = Trim(FirstName);
            LastName = Trim(LastName);

            Contragent contragent = new Contragent{FirstName = FirstName, LastName = LastName};
            Salesman salesman = new Salesman {ProfitPercentage = ProfitPercentage, SalaryPerDay = SalaryPerDay, Contragent = contragent};

            try
            {
                _mainViewModel.Context.Salesmen.Add(salesman);
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось добавить нового продавца"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new salesman" + e.Message);
            }

            _mainViewModel.SalesmenViewModel.AddSalesmanDialogViewModel = null;
        }

        private string Trim(string s)
        {
            if (s == null) return null;

            return s = s.Trim();
        }
    }
}
