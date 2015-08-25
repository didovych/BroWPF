using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels
{
    public class GuardViewModel : ContragentViewModel
    {
        public GuardViewModel(Guard guard)
            : base(guard.Contragent)
        {
        }

    }
}
