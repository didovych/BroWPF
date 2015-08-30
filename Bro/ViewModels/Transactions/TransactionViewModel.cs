using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels
{
    public class TransactionViewModel: ViewModelBase
    {
        public TransactionViewModel(Transaction transaction)
        {
            ID = transaction.ID;
            Type = (TranType)transaction.TypeID;
            Price = transaction.Price;
            Date = transaction.Date;

            var product = transaction.Product;
            if (product != null)
            {
                ProductID = product.ID;
                ModelName = product.Model.Name;
                CategoryName = product.Model.Category.Name;
                SerialNumber = product.SerialNumber;
            }

            if (transaction.Operator != null) Salesman = new SalesmanViewModel(transaction.Operator.Salesman);
            if (transaction.Contragent != null) ContragentLastName = new ContragentViewModel(transaction.Contragent);
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

        private TranType _type;

        public TranType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged();
            }
        }

        private int _productID;

        public int ProductID
        {
            get { return _productID; }
            set
            {
                _productID = value;
                NotifyPropertyChanged();
            }
        }

        private string _modelName;

        public string ModelName
        {
            get { return _modelName; }
            set
            {
                _modelName = value;
                NotifyPropertyChanged();
            }
        }

        private string _categoryName;

        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                NotifyPropertyChanged();
            }
        }

        private string _serialNumber;

        public string SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                _serialNumber = value;
                NotifyPropertyChanged();
            }
        }

        private decimal? _price;

        public decimal? Price
        {
            get { return _price; }
            set
            {
                _price = value;
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

        private ContragentViewModel _contragentLastName;

        public ContragentViewModel ContragentLastName
        {
            get { return _contragentLastName; }
            set
            {
                _contragentLastName = value;
                NotifyPropertyChanged();
            }
        }
    }
}
