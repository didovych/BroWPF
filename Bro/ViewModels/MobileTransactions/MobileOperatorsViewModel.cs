using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Bro.ViewModels.Dialogs;
using BroData;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.MobileTransactions
{
    public class MobileOperatorsViewModel: ViewModelBase
    {
        public MobileOperatorsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            MobileOperators = new ObservableCollection<MobileOperatorViewModel>(GetMobileOperators(mainViewModel.Context));
            MobileOperatorsView = CollectionViewSource.GetDefaultView(MobileOperators);

            AddCommand = new DelegateCommand(Add);
            CloseAddDialogCommand = new DelegateCommand(() => AddDialogViewModel = null);

            DeleteCommand = new DelegateCommand(Delete, () => SelectedOperator != null);
        }

        private MainViewModel _mainViewModel;

        private MobileOperatorViewModel _selectedOperator;

        public MobileOperatorViewModel SelectedOperator
        {
            get { return _selectedOperator; }
            set
            {
                _selectedOperator = value;
                NotifyPropertyChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        private AddMobileOperatorDialogViewModel _addDialogViewModel;

        public AddMobileOperatorDialogViewModel AddDialogViewModel
        {
            get { return _addDialogViewModel; }
            set
            {
                _addDialogViewModel = value;
                NotifyPropertyChanged();
            }
        }

        #region Delegate commands

        private DelegateCommand _addCommand;

        public DelegateCommand AddCommand
        {
            get { return _addCommand; }
            set
            {
                _addCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _closeAddDialogCommand;

        public DelegateCommand CloseAddDialogCommand
        {
            get { return _closeAddDialogCommand; }
            set
            {
                _closeAddDialogCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _deleteCommand;

        public DelegateCommand DeleteCommand
        {
            get { return _deleteCommand; }
            set
            {
                _deleteCommand = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Mobile operators
        private ObservableCollection<MobileOperatorViewModel> _mobileOperators;

        public ObservableCollection<MobileOperatorViewModel> MobileOperators
        {
            get { return _mobileOperators; }
            set
            {
                _mobileOperators = value;
                NotifyPropertyChanged();
            }
        }

        private ICollectionView _mobileOperatorsView;

        public ICollectionView MobileOperatorsView
        {
            get { return _mobileOperatorsView; }
            set
            {
                _mobileOperatorsView = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        private void Add()
        {
            AddDialogViewModel = new AddMobileOperatorDialogViewModel(_mainViewModel);
        }

        private void Delete()
        {
            if (SelectedOperator == null) return;

            MessageBoxResult answer = MessageBox.Show("Удалить выбранный оператор?", "Question", MessageBoxButton.YesNo);

            if (answer != MessageBoxResult.Yes) return;

            var operatorToDelete = _mainViewModel.Context.MobileOperators.ToList()
                .LastOrDefault(x => x.ID == SelectedOperator.ID);

            try
            {
                _mainViewModel.Context.MobileOperators.Remove(operatorToDelete);
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось удалить оператор", "Error", MessageBoxButton.OK);
                Logging.WriteToLog("Failed to remove mobile operator. " + e.Message);
            }

            Update();
        }

        public void Update()
        {
            MobileOperators.Clear();
            MobileOperators.AddRange(GetMobileOperators(_mainViewModel.Context));
        }

        private List<MobileOperatorViewModel> GetMobileOperators(Context context)
        {
            return
                context.MobileOperators.ToList().Select(x => new MobileOperatorViewModel(x)).ToList();
        }
    }
}
