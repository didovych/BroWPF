using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BroData;

namespace Bro.ViewModels
{
    public class SalesmenViewModel : ContragentsViewModel
    {
        public SalesmenViewModel(Context context)
            : base(context)
        {
        }

        protected override void AddContragent()
        {
            MessageBox.Show("New saleman was edited");
        }

        /// <summary>
        /// Edit saleman with SelectedContragent.ID
        /// </summary>
        protected override void EditContragent()
        {
            MessageBox.Show(String.Format("Saleman {0} was edited", SelectedContragent.ID));
        }

        protected override List<ContragentViewModel> GetContragents(Context context)
        {
            return context.Salesmen.ToList().Select(x => new SalesmanViewModel(x)).Cast<ContragentViewModel>().ToList();
        }
    }
}
