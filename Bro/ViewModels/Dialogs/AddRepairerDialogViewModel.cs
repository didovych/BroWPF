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
    public class AddRepairerDialogViewModel : ViewModelBase
    {
        public AddRepairerDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddRepairerCommand = new DelegateCommand(AddRepairer, Validate);
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
                AddRepairerCommand.RaiseCanExecuteChanged();
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
                AddRepairerCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand AddRepairerCommand { get; set; }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(FirstName)) return false;
            if (string.IsNullOrEmpty(LastName)) return false;

            return true;
        }

        private void AddRepairer()
        {
            FirstName = Trim(FirstName);
            LastName = Trim(LastName);

            Contragent contragent = new Contragent { FirstName = FirstName, LastName = LastName };
            Repairer repairer = new Repairer {Contragent = contragent};

            try
            {
                _mainViewModel.Context.Repairers.Add(repairer);
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось добавить нового мастера"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new repairer" + e.Message);
            }

            _mainViewModel.RepairersViewModel.Update();

            _mainViewModel.RepairersViewModel.AddRepairerDialogViewModel = null;
        }

        private string Trim(string s)
        {
            if (s == null) return null;

            return s = s.Trim();
        }
    }
}
