using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BroData;

namespace Bro.ViewModels
{
    public class RepairersViewModel : ContragentsViewModel
    {
        public RepairersViewModel(Context context) : base(context)
        {
        }

        protected override void AddContragent()
        {
            MessageBox.Show("New repairer was added");
        }

        protected override void EditContragent()
        {
            MessageBox.Show(String.Format("Repairer {0} was edited", SelectedContragent.ID));
        }

        protected override List<ContragentViewModel> GetContragents(Context context)
        {
            return context.Repairers.ToList().Select(x => new RepairerViewModel(x)).Cast<ContragentViewModel>().ToList();
        }
    }
}
