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
    public class EditSalesmanDialogViewModel : ViewModelBase
    {
        public EditSalesmanDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _salesmanToEdit = new SalesmanViewModel(mainViewModel.Context.Salesmen.FirstOrDefault(
                x => x.ID == mainViewModel.SalesmenViewModel.SelectedContragent.ID));

            EditSalesmanCommand = new DelegateCommand(EditSalesman, Validate);

            FirstName = _salesmanToEdit.FirstName;
            LastName = _salesmanToEdit.LastName;
            ProfitPercentage = _salesmanToEdit.ProfitPercentage;
            SalaryPerDay = _salesmanToEdit.SalaryPerDay;
        }

        private readonly MainViewModel _mainViewModel;

        private SalesmanViewModel _salesmanToEdit;

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyPropertyChanged();
                EditSalesmanCommand.RaiseCanExecuteChanged();
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
                EditSalesmanCommand.RaiseCanExecuteChanged();
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

        public DelegateCommand EditSalesmanCommand { get; set; }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(FirstName)) return false;
            if (string.IsNullOrEmpty(LastName)) return false;

            return true;
        }

        private void EditSalesman()
        {
            FirstName = Trim(FirstName);
            LastName = Trim(LastName);

            if (_salesmanToEdit == null)
            {
                MessageBox.Show(("Не удалось изменить данные продавца"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed edit salesman. There is no selected salesman.");
            }

            try
            {
                var salesmanRow = _mainViewModel.Context.Salesmen.FirstOrDefault(x => x.ID == _salesmanToEdit.ID);
                salesmanRow.ProfitPercentage = ProfitPercentage;
                salesmanRow.SalaryPerDay = SalaryPerDay;
                salesmanRow.Contragent.FirstName = FirstName;
                salesmanRow.Contragent.LastName = LastName;

                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось изменить данные продавца"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed edit salesman" + e.Message);
            }

            _mainViewModel.SalesmenViewModel.EditSalesmanDialogViewModel = null;
        }

        private string Trim(string s)
        {
            if (s == null) return null;

            return s = s.Trim();
        }
    }
}
