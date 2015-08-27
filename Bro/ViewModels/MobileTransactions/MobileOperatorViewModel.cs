using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels.MobileTransactions
{
    public class MobileOperatorViewModel: ViewModelBase
    {
        public MobileOperatorViewModel(MobileOperator mobileOperator)
        {
            ID = mobileOperator.ID;
            Name = mobileOperator.Name;
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

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }
    }
}
