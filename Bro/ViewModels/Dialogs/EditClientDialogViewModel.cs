using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class EditClientDialogViewModel : ViewModelBase
    {
        public EditClientDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            _clientToEdit = new ClientViewModel(mainViewModel.Context.Clients.FirstOrDefault(
                x => x.ID == mainViewModel.ClientsViewModel.SelectedContragent.ID));

            EditClientCommand = new DelegateCommand(EditClient, Validate);

            FirstName = _clientToEdit.FirstName;
            LastName = _clientToEdit.LastName;
        }

        private readonly MainViewModel _mainViewModel;

        private ClientViewModel _clientToEdit;

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyPropertyChanged();
                EditClientCommand.RaiseCanExecuteChanged();
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
                EditClientCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand EditClientCommand { get; set; }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(FirstName)) return false;
            if (string.IsNullOrEmpty(LastName)) return false;

            return true;
        }

        private void EditClient()
        {
            FirstName = Trim(FirstName);
            LastName = Trim(LastName);

            if (_clientToEdit == null)
            {
                MessageBox.Show(("Не удалось изменить данные клиента"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed edit client. There is no selected client.");
            }

            try
            {
                var clientRow = _mainViewModel.Context.Clients.FirstOrDefault(x => x.ID == _clientToEdit.ID);
                clientRow.Contragent.FirstName = FirstName;
                clientRow.Contragent.LastName = LastName;

                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось изменить данные клиента"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed edit client" + e.Message);
            }

            _mainViewModel.ClientsViewModel.Update();
            _mainViewModel.ClientsViewModel.EditClientDialogViewModel = null;
        }

        private string Trim(string s)
        {
            if (s == null) return null;

            return s = s.Trim();
        }
    }
}
