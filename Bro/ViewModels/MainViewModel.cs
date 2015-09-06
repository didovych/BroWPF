using System; 
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Bro.Services;
using Bro.ViewModels.Dialogs;
using Bro.ViewModels.MobileTransactions;
using Bro.ViewModels.ProductsViewModels;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            _context = new Context();

            UpdateCashInHand();

            SoldProductsViewModel = new SoldProductsViewModel(this);
            OnStockProductsViewModel = new OnStockProductsViewModel(this);
            ToRepairProductsViewModel = new ToRepairProductsViewModel(this);
            ToPawnProductsViewModel = new ToPawnProductsViewModel(this);

            SalesmenViewModel = new SalesmenViewModel(this);
            ClientsViewModel = new ClientsViewModel(this);
            RepairersViewModel = new RepairersViewModel(this);
            GuardsViewModel = new GuardsViewModel(this);

            CashTransactionsViewModel = new CashTransactionsViewModel(this);

            MobileOperatorsViewModel = new MobileOperatorsViewModel(this);
            MobileTransactionsViewModel = new MobileTransactionsViewModel(this);

            ReportsViewModel = new ReportsViewModel(_context);

            IsUserAdmin = OperatorManager.Instance.IsUserAdmin;
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

        public bool IsUserAdmin { get; set; }

        private decimal _cashInHand;

        public decimal CashInHand
        {
            get { return _cashInHand; }
            set
            {
                _cashInHand = value;
                NotifyPropertyChanged();
            }
        }

        private decimal _productsValue;

        public decimal ProductsValue
        {
            get { return _productsValue; }
            set
            {
                _productsValue = value;
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

        private GuardsViewModel _guardsViewModel;

        public GuardsViewModel GuardsViewModel
        {
            get { return _guardsViewModel; }
            set
            {
                _guardsViewModel = value;
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

        private MobileOperatorsViewModel _mobileOperatorsViewModel;

        public MobileOperatorsViewModel MobileOperatorsViewModel
        {
            get { return _mobileOperatorsViewModel; }
            set
            {
                _mobileOperatorsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private MobileTransactionsViewModel _mobileTransactionsViewModel;

        public MobileTransactionsViewModel MobileTransactionsViewModel
        {
            get { return _mobileTransactionsViewModel; }
            set
            {
                _mobileTransactionsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        private ReportsViewModel _reportsViewModel;

        public ReportsViewModel ReportsViewModel
        {
            get { return _reportsViewModel; }
            set
            {
                _reportsViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public void UpdateCashInHand()
        {
            CashInHand = 0;
            ProductsValue = 0;

            List<Transaction> cashTransactions =
                Context.Transactions.Where(
                    x =>
                        x.TypeID == (int) TranType.CashIn || x.TypeID == (int) TranType.CashOut ||
                        x.TypeID == (int) TranType.Salary || x.TypeID == (int) TranType.TopUp ||
                        x.TypeID == (int) TranType.Coffee).ToList();

            List<Transaction> soldProductsTransactions = Context.Products.Where(x => x.Transactions.Any()).ToList().
                Where(x => x.Transactions.OrderBy(y => y.Date).Last().TypeID == (int) TranType.Sold)
                .ToList()
                .SelectMany(x => x.Transactions)
                .ToList();

            List<Transaction> notSoldProductsTransactions = Context.Products.Where(x => x.Transactions.Any()).ToList().
                Where(x => x.Transactions.OrderBy(y => y.Date).Last().TypeID != (int)TranType.Sold)
                .ToList()
                .SelectMany(x => x.Transactions)
                .ToList();

            foreach (var transaction in cashTransactions)
            {
                if (transaction.Price == null) continue;

                switch (transaction.TypeID)
                {
                    case (int) TranType.CashIn:
                        CashInHand += transaction.Price.Value;
                        break;
                    case (int)TranType.CashOut:
                        CashInHand -= transaction.Price.Value;
                        break;
                    case (int)TranType.Salary:
                        CashInHand -= transaction.Price.Value;
                        break;
                    case (int)TranType.Coffee:
                        CashInHand += transaction.Price.Value;
                        break;
                    case (int)TranType.TopUp:
                        CashInHand += transaction.Price.Value;
                        break;
                }
            }

            foreach (var transaction in soldProductsTransactions)
            {
                if (transaction.Price == null) continue;

                switch (transaction.TypeID)
                {
                    case (int)TranType.Sold:
                        CashInHand += transaction.Price.Value;
                        break;
                    case (int)TranType.Bought:
                        CashInHand -= transaction.Price.Value;
                        break;
                    case (int)TranType.Repaired:
                        CashInHand -= transaction.Price.Value;
                        break;
                    case (int)TranType.ToPawn:
                        CashInHand -= transaction.Price.Value;
                        break;
                }
            }

            foreach (var transaction in notSoldProductsTransactions)
            {
                if (transaction.Price == null) continue;

                CashInHand -= transaction.Price.Value;
                ProductsValue += transaction.Price.Value;
            }
        }
    }
}
