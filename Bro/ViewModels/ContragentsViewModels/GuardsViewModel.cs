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
    public class GuardsViewModel : ContragentsViewModel
    {
        public GuardsViewModel(MainViewModel mainViewModel)
            : base(mainViewModel.Context)
        {
            _mainViewModel = mainViewModel;

            CloseAddGuardDialogCommand = new DelegateCommand(() => AddGuardDialogViewModel = null);
            CloseEditGuardDialogCommand = new DelegateCommand(() => EditGuardDialogViewModel = null);
        }

        private readonly MainViewModel _mainViewModel;

        private DelegateCommand _closeAddGuardDialogCommand;

        public DelegateCommand CloseAddGuardDialogCommand
        {
            get { return _closeAddGuardDialogCommand; }
            set
            {
                _closeAddGuardDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closeEditGuardDialogCommand;

        public DelegateCommand CloseEditGuardDialogCommand
        {
            get { return _closeEditGuardDialogCommand; }
            set
            {
                _closeEditGuardDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private AddGuardDialogViewModel _addGuardDialogViewModel;

        public AddGuardDialogViewModel AddGuardDialogViewModel
        {
            get { return _addGuardDialogViewModel; }
            set
            {
                _addGuardDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private EditGuardDialogViewModel _editGuardDialogViewModel;

        public EditGuardDialogViewModel EditGuardDialogViewModel
        {
            get { return _editGuardDialogViewModel; }
            set
            {
                _editGuardDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        protected override void AddContragent()
        {
            AddGuardDialogViewModel = new AddGuardDialogViewModel(_mainViewModel);
        }

        protected override void EditContragent()
        {
            EditGuardDialogViewModel = new EditGuardDialogViewModel(_mainViewModel);
        }


        protected override List<ContragentViewModel> GetContragents(Context context)
        {
            return context.Guards.ToList().Select(x => new GuardViewModel(x)).Cast<ContragentViewModel>().ToList();
        }
    }
}
