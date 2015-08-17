using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Bro.ViewModels.ProductsViewModels;
using BroData;

namespace Bro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            _context = new Context();

            SoldProductsViewModel = new SoldProductsViewModel(_context);
            OnStockProductsViewModel = new OnStockProductsViewModel(_context);
            ToRepairProductsViewModel = new ToRepairProductsViewModel(_context);
            ToPawnProductsViewModel = new ToPawnProductsViewModel(_context);

            SalesmenViewModel = new SalesmenViewModel(_context);
            ClientsViewModel = new ClientsViewModel(_context);
            RepairersViewModel = new RepairersViewModel(_context);

            CashTransactionsViewModel = new CashTransactionsViewModel(_context);
        }

        private Context _context;

        public Context Context
        {
            get { return _context; }
            set
            {
                _context = value;
                NotifyPropertyChanged();
            }
        }

        private SoldProductsViewModel _soldProductsViewModel;

        public SoldProductsViewModel SoldProductsViewModel
        {
            get { return _soldProductsViewModel; }
            set
            {
                _soldProductsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private OnStockProductsViewModel _onStockProductsViewModel;

        public OnStockProductsViewModel OnStockProductsViewModel
        {
            get { return _onStockProductsViewModel; }
            set
            {
                _onStockProductsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private ToRepairProductsViewModel _toRepairProductsViewModel;

        public ToRepairProductsViewModel ToRepairProductsViewModel
        {
            get { return _toRepairProductsViewModel; }
            set
            {
                _toRepairProductsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private ToPawnProductsViewModel _toPawnProductsViewModel;

        public ToPawnProductsViewModel ToPawnProductsViewModel
        {
            get { return _toPawnProductsViewModel; }
            set
            {
                _toPawnProductsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private SalesmenViewModel _salesmenViewModel;

        public SalesmenViewModel SalesmenViewModel
        {
            get { return _salesmenViewModel; }
            set
            {
                _salesmenViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private ClientsViewModel _clientsViewModel;

        public ClientsViewModel ClientsViewModel
        {
            get { return _clientsViewModel; }
            set
            {
                _clientsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private RepairersViewModel _repairersViewModel;

        public RepairersViewModel RepairersViewModel
        {
            get { return _repairersViewModel; }
            set
            {
                _repairersViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private CashTransactionsViewModel _cashTransactionsViewModel;

        public CashTransactionsViewModel CashTransactionsViewModel
        {
            get { return _cashTransactionsViewModel; }
            set
            {
                _cashTransactionsViewModel = value;
                NotifyPropertyChanged();
            }
        }
    }
}
