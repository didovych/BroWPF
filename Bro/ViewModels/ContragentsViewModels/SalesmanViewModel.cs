using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels
{
    public class SalesmanViewModel : ContragentViewModel
    {
        public SalesmanViewModel(Salesman salesman) : base(salesman.Contragent)
        {
            ProfitPercentage = salesman.ProfitPercentage;
            SalaryPerDay = salesman.SalaryPerDay;
            Login = salesman.Login;
        }

        private int _profitPercentage;

        public int ProfitPercentage
        {
            get { return _profitPercentage; }
            set
            {
                _profitPercentage = value;
                NotifyPropertyChanged();
            }
        }

        private int _salaryPerDay;

        public int SalaryPerDay
        {
            get { return _salaryPerDay; }
            set
            {
                _salaryPerDay = value;
                NotifyPropertyChanged();
            }
        }

        private string _login;

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                NotifyPropertyChanged();
            }
        }
    }
}
