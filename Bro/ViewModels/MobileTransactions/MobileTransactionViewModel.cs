using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels.MobileTransactions
{
    public class MobileTransactionViewModel: ViewModelBase
    {
        public MobileTransactionViewModel()
        {
        }

        public MobileTransactionViewModel(MobileTransaction mobileTransaction)
        {
            ID = mobileTransaction.ID;
            MobileOperator = new MobileOperatorViewModel(mobileTransaction.MobileOperator);
            CreditSum = mobileTransaction.CreditSum;
            if (mobileTransaction.Transaction.Price != null) Price = mobileTransaction.Transaction.Price.Value;
            if (mobileTransaction.Transaction.Contragent != null) Client = new ContragentViewModel(mobileTransaction.Transaction.Contragent);
            Salesman = new SalesmanViewModel(mobileTransaction.Transaction.Operator.Salesman);
            Date = mobileTransaction.Transaction.Date;
            Profit = Price - CreditSum;
        }

        private int _id;

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        private MobileOperatorViewModel _mobileOperator;

        public MobileOperatorViewModel MobileOperator
        {
            get { return _mobileOperator; }
            set
            {
                _mobileOperator = value;
                NotifyPropertyChanged();
            }
        }

        private decimal _creditSum;

        public decimal CreditSum
        {
            get { return _creditSum; }
            set
            {
                _creditSum = value;
                NotifyPropertyChanged();
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
            }
        }

        private decimal _profit;

        public decimal Profit
        {
            get { return _profit; }
            set
            {
                _profit = value;
                NotifyPropertyChanged();
            }
        }

        private ContragentViewModel _client;

        public ContragentViewModel Client
        {
            get { return _client; }
            set
            {
                _client = value;
                NotifyPropertyChanged();
            }
        }

        private SalesmanViewModel _salesman;

        public SalesmanViewModel Salesman
        {
            get { return _salesman; }
            set
            {
                _salesman = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                NotifyPropertyChanged();
            }
        }
    }
}
