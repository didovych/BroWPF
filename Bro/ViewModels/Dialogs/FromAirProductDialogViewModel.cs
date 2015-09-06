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
    public class FromAirProductDialogViewModel : ViewModelBase
    {
        public FromAirProductDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _selectedProductIDs = _mainViewModel.OnStockProductsViewModel.SelectedProduct.IDs;

            FromAirProductCommand = new DelegateCommand(FromAirProduct, Validate);

            Number = 1;
        }

        private readonly MainViewModel _mainViewModel;
        private readonly List<int> _selectedProductIDs;

        private int _number;

        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                NotifyPropertyChanged();
                FromAirProductCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand FromAirProductCommand { get; set; }

        private void FromAirProduct()
        {
            for (int i = 0; i < Number; i++)
                _mainViewModel.Context.Transactions.Add(new Transaction
                {
                    ProductID = _selectedProductIDs[i],
                    Date = DateTime.Now,
                    TypeID = (int)TranType.Bought,
                    OperatorID = OperatorManager.Instance.CurrentUserID,
                });

            try
            {
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(("Не удалось забрать товар с выносной."), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Logging.WriteToLog("Failed from air OnStock product. " + exception.Message);
            }

            _mainViewModel.OnStockProductsViewModel.Update();

            _mainViewModel.OnStockProductsViewModel.FromAirProductDialogViewModel = null;
        }

        private bool Validate()
        {
            return (Number >= 1 && Number <= _selectedProductIDs.Count) ;
        }
    }
}
