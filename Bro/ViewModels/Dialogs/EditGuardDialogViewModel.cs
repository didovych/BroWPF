using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class EditGuardDialogViewModel : ViewModelBase
    {
        public EditGuardDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _guardToEdit =
                new GuardViewModel(
                    mainViewModel.Context.Guards.FirstOrDefault(
                        x => x.ID == mainViewModel.GuardsViewModel.SelectedContragent.ID));

            EditGuardCommand = new DelegateCommand(EditGuard, Validate);

            FirstName = _guardToEdit.FirstName;
            LastName = _guardToEdit.LastName;
        }

        private readonly MainViewModel _mainViewModel;
        private readonly GuardViewModel _guardToEdit;

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyPropertyChanged();
                EditGuardCommand.RaiseCanExecuteChanged();
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
                EditGuardCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand EditGuardCommand { get; set; }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(FirstName)) return false;
            if (string.IsNullOrEmpty(LastName)) return false;

            return true;
        }

        private void EditGuard()
        {
            FirstName = Trim(FirstName);
            LastName = Trim(LastName);

            if (_guardToEdit == null)
            {
                MessageBox.Show(("Не удалось изменить данные охранника"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed edit guard. There is no selected guard.");
            }

            try
            {
                var guardRow = _mainViewModel.Context.Guards.FirstOrDefault(x => x.ID == _guardToEdit.ID);
                guardRow.Contragent.FirstName = FirstName;
                guardRow.Contragent.LastName = LastName;

                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось изменить данные охранника"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed edit guard" + e.Message);
            }

            _mainViewModel.GuardsViewModel.Update();
            _mainViewModel.GuardsViewModel.EditGuardDialogViewModel = null;
        }

        private string Trim(string s)
        {
            if (s == null) return null;

            return s = s.Trim();
        }
    }
}
