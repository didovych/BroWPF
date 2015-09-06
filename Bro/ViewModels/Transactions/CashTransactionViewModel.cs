using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BroData;

namespace Bro.ViewModels
{
    public class CashTransactionViewModel : ViewModelBase, IPriceModel
    {
        public CashTransactionViewModel(Transaction transaction)
        {
            ID = transaction.ID;
            Date = transaction.Date;
            CashTranType = (TranType) transaction.TypeID;
            Salesman = new SalesmanViewModel(transaction.Operator.Salesman);
            if (transaction.Contragent != null) Contragent = new ContragentViewModel(transaction.Contragent);
            if (transaction.Price != null) Price = transaction.Price.Value;
            Notes = transaction.Notes;
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

        private TranType _cashTranType;

        public TranType CashTranType
        {
            get { return _cashTranType; }
            set
            {
                _cashTranType = value;
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

        private string _notes;

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                NotifyPropertyChanged();
            }
        }

        private ContragentViewModel _contragent;

        public ContragentViewModel Contragent
        {
            get { return _contragent; }
            set
            {
                _contragent = value;
                NotifyPropertyChanged();
            }
        }
    }
}
