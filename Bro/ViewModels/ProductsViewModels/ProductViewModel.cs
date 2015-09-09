using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels
{
    public abstract class ProductViewModel : ViewModelBase, IPriceModel
    {
        protected ProductViewModel(Product product)
        {
            IDs = new List<int>{product.ID};
            SerialNumber = product.SerialNumber;
            ModelName = product.Model.Name;
            CategoryName = product.Model.Category.Name;
            Notes = product.Notes;
            SellingPrice = product.SellingPrice;
            DateSellTo = product.DateSellTo;
            ModelID = product.ModelID;

            Origin = TranType.Zero;
            MoneySpentForProduct = 0;

            var orderedTransactions = product.Transactions.OrderBy(x => x.Date);

            if (orderedTransactions.FirstOrDefault() != null && orderedTransactions.FirstOrDefault().Operator != null) SalesmanBought = new SalesmanViewModel(orderedTransactions.FirstOrDefault().Operator.Salesman);

            foreach (var transaction in orderedTransactions)
            {
                if (transaction.TypeID != (int)TranType.Bought && transaction.TypeID != (int)TranType.Repaired &&
                    transaction.TypeID != (int)TranType.ToPawn && transaction.TypeID != (int)TranType.ToRepair) continue;

                if (transaction.TypeID != (int)TranType.ToRepair && transaction.Price != null) MoneySpentForProduct += transaction.Price.Value;

                if (transaction.TypeID != (int)TranType.Repaired) Origin = (TranType)transaction.TypeID;
            }

            Status = orderedTransactions.LastOrDefault() == null ? TranType.Zero : (TranType) orderedTransactions.LastOrDefault().TypeID;
        }

        private List<int> _ids;

        public List<int> IDs
        {
            get { return _ids; }
            set
            {
                _ids = value;
                NotifyPropertyChanged();
            }
        }

        private TranType _status;

        /// <summary>
        /// Status of the product's last transaction
        /// </summary>
        public TranType Status
        {
            get { return _status; }
            set
            {
                _status = value;
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

        private int _modelID;

        public int ModelID
        {
            get { return _modelID; }
            set
            {
                _modelID = value;
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

        private decimal? _sellingPrice;

        public decimal? SellingPrice
        {
            get { return _sellingPrice; }
            set
            {
                _sellingPrice = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime? _dateSellTo;

        public DateTime? DateSellTo
        {
            get { return _dateSellTo; }
            set
            {
                _dateSellTo = value;
                NotifyPropertyChanged();
            }
        }

        private TranType _origin;

        public TranType Origin
        {
            get { return _origin; }
            set
            {
                _origin = value;
                NotifyPropertyChanged();
            }
        }

        private decimal _moneySpentForProduct;

        public decimal MoneySpentForProduct
        {
            get { return _moneySpentForProduct; }
            set
            {
                _moneySpentForProduct = value;
                NotifyPropertyChanged();
            }
        }

        private SalesmanViewModel _salesmanBought;

        public SalesmanViewModel SalesmanBought
        {
            get { return _salesmanBought; }
            set
            {
                _salesmanBought = value;
                NotifyPropertyChanged();
            }
        }

        public decimal Price { get { return MoneySpentForProduct * IDs.Count; } }
    }
}
