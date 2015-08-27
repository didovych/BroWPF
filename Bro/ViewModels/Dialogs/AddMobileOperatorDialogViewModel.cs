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
    public class AddMobileOperatorDialogViewModel : ViewModelBase
    {
        public AddMobileOperatorDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddCommand = new DelegateCommand(Add, Validate);
        }

        private MainViewModel _mainViewModel;

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
                AddCommand.RaiseCanExecuteChanged();
            }
        }

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

        private bool Validate()
        {
            if (string.IsNullOrEmpty(Name)) return false;

            return true;
        }

        private string Trim(string s)
        {
            if (s == null) return null;

            return s = s.Trim();
        }

        private void Add()
        {
            Name = Trim(Name);

            MobileOperator mobileOperator = new MobileOperator{Name = Name};

            _mainViewModel.Context.MobileOperators.Add(mobileOperator);

            try
            {
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось добавить оператор"), "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new mobile operator" + e.Message);
            }

            _mainViewModel.MobileOperatorsViewModel.Update();
            _mainViewModel.MobileOperatorsViewModel.AddDialogViewModel = null;
        }
    }
}
