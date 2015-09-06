using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bro.Services;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class AddMobileTransactionDialogViewModel : ViewModelBase
    {
        public AddMobileTransactionDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            AddCommand = new DelegateCommand(Add, Validate);

            Operators = _mainViewModel.Context.MobileOperators.ToList();
            SelectedOperator = Operators.FirstOrDefault();
        }

        private MainViewModel _mainViewModel;

        private List<MobileOperator> _operators;

        public List<MobileOperator> Operators
        {
            get { return _operators; }
            set
            {
                _operators = value;
                NotifyPropertyChanged();
            }
        }

        private MobileOperator _selectedOperator;

        public MobileOperator SelectedOperator
        {
            get { return _selectedOperator; }
            set
            {
                _selectedOperator = value;
                NotifyPropertyChanged();
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal _creditSum ;

        public decimal CreditSum
        {
            get { return _creditSum; }
            set
            {
                _creditSum = value;
                NotifyPropertyChanged();
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyPropertyChanged();
                AddCommand.RaiseCanExecuteChanged();
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
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
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
            if (CreditSum <= 0) return false;
            if (Price < 0) return false;
            if (SelectedOperator == null) return false;

            return true;
        }

        private string Trim(string s)
        {
            if (s == null) return null;

            return s = s.Trim();
        }

        private void Add()
        {
            if (SelectedOperator == null) _mainViewModel.MobileTransactionsViewModel.AddDialogViewModel = null;

            Transaction transaction = new Transaction
            {
                Date = DateTime.Now,
                TypeID = (int) TranType.TopUp,
                OperatorID = OperatorManager.Instance.CurrentUserID,
                Price = Price
            };

            LastName = Trim(LastName);
            FirstName = Trim(FirstName);
            if (!string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(FirstName))
            {
                var client =_mainViewModel.Context.Clients.FirstOrDefault(
                    x => x.Contragent.FirstName == FirstName && x.Contragent.LastName == LastName);

                if (client == null)
                {
                    var contragent = new Contragent
                    {
                        LastName = LastName,
                        FirstName = FirstName
                    };

                    client = new Client
                    {
                        Contragent = contragent
                    };

                    _mainViewModel.Context.Clients.Add(client);
                }

                transaction.Contragent = client.Contragent;
            }

            MobileTransaction mobileTransaction = new MobileTransaction
            {
                Transaction = transaction,
                CreditSum = CreditSum,
                MobileOperatorID = SelectedOperator.ID
            };

            _mainViewModel.Context.MobileTransactions.Add(mobileTransaction);
            try
            {
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось провести пополнение"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new mobile transaction. " + e.Message);
            }

            _mainViewModel.MobileTransactionsViewModel.Update();
            _mainViewModel.CashInHand += Price;

            _mainViewModel.MobileTransactionsViewModel.AddDialogViewModel = null;
        }
    }
}
