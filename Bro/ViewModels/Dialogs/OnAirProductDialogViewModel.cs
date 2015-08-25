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
    public class OnAirProductDialogViewModel: ViewModelBase
    {
        public OnAirProductDialogViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _selectedProductIDs = _mainViewModel.OnStockProductsViewModel.SelectedProduct.IDs;

            OnAirProductCommand = new DelegateCommand(OnAirProduct, Validate);

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
                OnAirProductCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand OnAirProductCommand { get; set; }

        private void OnAirProduct()
        {
            for (int i = 0; i < Number; i++)
                _mainViewModel.Context.Transactions.Add(new Transaction
                {
                    ProductID = _selectedProductIDs[i],
                    Date = DateTime.Now,
                    TypeID = (int)TranType.OnAir,
                    OperatorID = 1,
                });

            try
            {
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(("Не удалось передать товар на выносную."), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Logging.WriteToLog("Failed on air OnStock product. " + exception.Message);
            }

            _mainViewModel.OnStockProductsViewModel.Update();

            _mainViewModel.OnStockProductsViewModel.OnAirProductDialogViewModel = null;
        }

        private bool Validate()
        {
            return (Number >= 1 && Number <= _selectedProductIDs.Count) ;
        }
    }
}
