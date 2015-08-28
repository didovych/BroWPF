using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bro.ViewModels;
using Bro.ViewModels.ProductsViewModels;
using BroData;

namespace Bro.Helpers
{
    public class ModelSerialNumberStatusPriceBoughtGroup
    {
        public ModelSerialNumberStatusPriceBoughtGroup(Product product)
        {
            Model = product.Model;
            SerialNumber = product.SerialNumber;

            var orderedTransactions = product.Transactions.OrderBy(x => x.Date);
            Status = orderedTransactions.LastOrDefault() == null ? TranType.Zero : (TranType)orderedTransactions.LastOrDefault().TypeID;
            PriceBought = orderedTransactions.FirstOrDefault(x => x.TypeID == (int) TranType.Bought) == null
                ? null
                : orderedTransactions.FirstOrDefault(x => x.TypeID == (int)TranType.Bought).Price;
        }

        public Model Model { get; set; }
        public string SerialNumber { get; set; }
        public TranType Status { get; set; }
        public decimal? PriceBought { get; set; }

        public override bool Equals(object obj)
        {
            var toCompare = obj as ModelSerialNumberStatusPriceBoughtGroup;

            if (toCompare == null) return false;

            return (Model.GetHashCode() == toCompare.Model.GetHashCode() && SerialNumber == toCompare.SerialNumber &&
                    Status == toCompare.Status && PriceBought == toCompare.PriceBought);
        }

        public override int GetHashCode()
        {
            var result = (Model == null ? 0 : Model.GetHashCode());
            result += 17*(SerialNumber == null ? 0 : SerialNumber.GetHashCode());
            result += 17*17 * Status.GetHashCode();
            result += PriceBought == null ? 0 : 17*17*17*(int)PriceBought;
            return result;
        }
    }
}
