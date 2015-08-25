using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class EditRepairerDialogViewModel : ViewModelBase
    {
        public EditRepairerDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _repairerToEdit =
                new RepairerViewModel(
                    mainViewModel.Context.Repairers.FirstOrDefault(
                        x => x.ID == mainViewModel.RepairersViewModel.SelectedContragent.ID));

            EditRepairerCommand = new DelegateCommand(EditRepairer, Validate);

            FirstName = _repairerToEdit.FirstName;
            LastName = _repairerToEdit.LastName;
        }

        private readonly MainViewModel _mainViewModel;
        private readonly RepairerViewModel _repairerToEdit;

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyPropertyChanged();
                EditRepairerCommand.RaiseCanExecuteChanged();
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
                EditRepairerCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand EditRepairerCommand { get; set; }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(FirstName)) return false;
            if (string.IsNullOrEmpty(LastName)) return false;

            return true;
        }

        private void EditRepairer()
        {
            FirstName = Trim(FirstName);
            LastName = Trim(LastName);

            if (_repairerToEdit == null)
            {
                MessageBox.Show(("Не удалось изменить данные мастера"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed edit repairer. There is no selected repairer.");
            }

            try
            {
                var repairerRow = _mainViewModel.Context.Repairers.FirstOrDefault(x => x.ID == _repairerToEdit.ID);
                repairerRow.Contragent.FirstName = FirstName;
                repairerRow.Contragent.LastName = LastName;

                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось изменить данные мастера"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed edit repairer" + e.Message);
            }

            _mainViewModel.RepairersViewModel.Update();
            _mainViewModel.RepairersViewModel.EditRepairerDialogViewModel = null;
        }

        private string Trim(string s)
        {
            if (s == null) return null;

            return s = s.Trim();
        }
    }
}
