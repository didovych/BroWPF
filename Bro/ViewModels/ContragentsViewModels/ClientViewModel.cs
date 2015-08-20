using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.ViewModels
{
    public class ClientViewModel : ContragentViewModel
    {
        public ClientViewModel(Client client) : base(client.Contragent)
        {
        }

    }
}
