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
    public class AddGuardDialogViewModel : ViewModelBase
    {
        public AddGuardDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddGuardCommand = new DelegateCommand(AddGuard, Validate);
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
                AddGuardCommand.RaiseCanExecuteChanged();
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
                AddGuardCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand AddGuardCommand { get; set; }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(FirstName)) return false;
            if (string.IsNullOrEmpty(LastName)) return false;

            return true;
        }

        private void AddGuard()
        {
            FirstName = Trim(FirstName);
            LastName = Trim(LastName);

            Contragent contragent = new Contragent { FirstName = FirstName, LastName = LastName };
            Guard guard = new Guard { Contragent = contragent };

            try
            {
                _mainViewModel.Context.Guards.Add(guard);
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось добавить нового мастера"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new repairer" + e.Message);
            }

            _mainViewModel.GuardsViewModel.Update();

            _mainViewModel.GuardsViewModel.AddGuardDialogViewModel = null;
        }

        private string Trim(string s)
        {
            if (s == null) return null;

            return s = s.Trim();
        }
    }
}
