using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels
{
    public class RepairerViewModel : ContragentViewModel
    {
        public RepairerViewModel(Repairer repairer) : base(repairer.Contragent)
        {
        }
    }
}
