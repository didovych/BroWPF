﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels
{
    public class CashTransactionViewModel : ViewModelBase
    {
        public CashTransactionViewModel(Transaction transaction)
        {
            ID = transaction.ID;
            Date = transaction.Date;
            CashTranType = (TranType) transaction.TransactionType.ID;
            Salesman = new SalesmanViewModel(transaction.Operator.Salesman);
            if (transaction.Price != null) Price = transaction.Price.Value;
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
    }
}
