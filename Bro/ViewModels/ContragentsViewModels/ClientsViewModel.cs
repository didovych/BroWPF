using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bro.ViewModels.Dialogs;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels
{
    public class ClientsViewModel : ContragentsViewModel
    {
        public ClientsViewModel(MainViewModel mainViewModel) : base(mainViewModel.Context)
        {
            _mainViewModel = mainViewModel;

            CloseEditClientDialogCommand = new DelegateCommand(() => EditClientDialogViewModel = null);
        }

        private readonly MainViewModel _mainViewModel;

        private DelegateCommand _closeEditClientDialogCommand;

        public DelegateCommand CloseEditClientDialogCommand
        {
            get { return _closeEditClientDialogCommand; }
            set
            {
                _closeEditClientDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private EditClientDialogViewModel _editClientDialogViewModel;

        public EditClientDialogViewModel EditClientDialogViewModel
        {
            get { return _editClientDialogViewModel; }
            set
            {
                _editClientDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        protected override void AddContragent()
        {
            MessageBox.Show("New client was edited");
        }

        protected override void EditContragent()
        {
            EditClientDialogViewModel = new EditClientDialogViewModel(_mainViewModel);
        }

        protected override List<ContragentViewModel> GetContragents(Context context)
        {
            return context.Clients.ToList().Select(x => new ClientViewModel(x)).Cast<ContragentViewModel>().ToList();
        }
    }
}
