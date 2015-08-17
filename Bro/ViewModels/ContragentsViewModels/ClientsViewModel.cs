using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BroData;

namespace Bro.ViewModels
{
    public class ClientsViewModel : ContragentsViewModel
    {
        public ClientsViewModel(Context context) : base(context)
        {
        }

        protected override void AddContragent()
        {
            MessageBox.Show("New client was edited");
        }

        protected override void EditContragent()
        {
            MessageBox.Show(String.Format("Client {0} was edited", SelectedContragent.ID));
        }

        protected override List<ContragentViewModel> GetContragents(Context context)
        {
            return context.Clients.ToList().Select(x => new ClientViewModel(x)).Cast<ContragentViewModel>().ToList();
        }
    }
}
