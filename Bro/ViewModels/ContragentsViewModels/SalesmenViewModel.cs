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
    public class SalesmenViewModel : ContragentsViewModel
    {
        public SalesmenViewModel(MainViewModel mainViewModel)
            : base(mainViewModel.Context)
        {
            _mainViewModel = mainViewModel;

            CloseAddSalesmanDialogCommand = new DelegateCommand(() => AddSalesmanDialogViewModel = null);
            CloseEditSalesmanDialogCommand = new DelegateCommand(() => EditSalesmanDialogViewModel = null);
        }

        private readonly MainViewModel _mainViewModel;

        private DelegateCommand _closeAddSalesmanDialogCommand;

        public DelegateCommand CloseAddSalesmanDialogCommand
        {
            get { return _closeAddSalesmanDialogCommand; }
            set
            {
                _closeAddSalesmanDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        DelegateCommand _closeEditSalesmanDialogCommand;

        public DelegateCommand CloseEditSalesmanDialogCommand
        {
            get { return _closeEditSalesmanDialogCommand; }
            set
            {
                _closeEditSalesmanDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private AddSalesmanDialogViewModel _addSalesmanDialogViewModel;

        public AddSalesmanDialogViewModel AddSalesmanDialogViewModel
        {
            get { return _addSalesmanDialogViewModel; }
            set
            {
                _addSalesmanDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private EditSalesmanDialogViewModel _editSalesmanDialogViewModel;

        public EditSalesmanDialogViewModel EditSalesmanDialogViewModel
        {
            get { return _editSalesmanDialogViewModel; }
            set
            {
                _editSalesmanDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        protected override void AddContragent()
        {
            AddSalesmanDialogViewModel = new AddSalesmanDialogViewModel(_mainViewModel);
        }

        /// <summary>
        /// Edit saleman with SelectedContragent.ID
        /// </summary>
        protected override void EditContragent()
        {
            EditSalesmanDialogViewModel = new EditSalesmanDialogViewModel(_mainViewModel);
        }

        protected override List<ContragentViewModel> GetContragents(Context context)
        {
            return context.Salesmen.ToList().Select(x => new SalesmanViewModel(x)).Cast<ContragentViewModel>().ToList();
        }
    }
}
