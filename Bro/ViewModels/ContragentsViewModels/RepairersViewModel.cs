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
    public class RepairersViewModel : ContragentsViewModel
    {
        public RepairersViewModel(MainViewModel mainViewModel) : base(mainViewModel.Context)
        {
            _mainViewModel = mainViewModel;

            CloseAddRepairerDialogCommand = new DelegateCommand(() => AddRepairerDialogViewModel = null);
            CloseEditRepairerDialogCommand = new DelegateCommand(() => EditRepairerDialogViewModel = null);
        }

        private readonly MainViewModel _mainViewModel;

        private DelegateCommand _closeAddRepairerDialogCommand;

        public DelegateCommand CloseAddRepairerDialogCommand
        {
            get { return _closeAddRepairerDialogCommand; }
            set
            {
                _closeAddRepairerDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closeEditRepairerDialogCommand;

        public DelegateCommand CloseEditRepairerDialogCommand
        {
            get { return _closeEditRepairerDialogCommand; }
            set
            {
                _closeEditRepairerDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private AddRepairerDialogViewModel _addRepairerDialogViewModel;

        public AddRepairerDialogViewModel AddRepairerDialogViewModel
        {
            get { return _addRepairerDialogViewModel; }
            set
            {
                _addRepairerDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private EditRepairerDialogViewModel _editRepairerDialogViewModel;

        public EditRepairerDialogViewModel EditRepairerDialogViewModel
        {
            get { return _editRepairerDialogViewModel; }
            set
            {
                _editRepairerDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        protected override void AddContragent()
        {
            AddRepairerDialogViewModel = new AddRepairerDialogViewModel(_mainViewModel);
        }

        protected override void EditContragent()
        {
            EditRepairerDialogViewModel = new EditRepairerDialogViewModel(_mainViewModel);
        }

        protected override List<ContragentViewModel> GetContragents(Context context)
        {
            return context.Repairers.ToList().Select(x => new RepairerViewModel(x)).Cast<ContragentViewModel>().ToList();
        }
    }
}
