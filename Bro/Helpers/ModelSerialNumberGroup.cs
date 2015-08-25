using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroData;

namespace Bro.Helpers
{
    public class ModelSerialNumberGroup
    {
        public ModelSerialNumberGroup(Product product)
        {
            Model = product.Model;
            SerialNumber = product.SerialNumber;
        }

        public Model Model { get; set; }
        public string SerialNumber { get; set; }

        public override bool Equals(object obj)
        {
            var toCompare = obj as ModelSerialNumberGroup;

            if (toCompare == null) return false;

            return GetHashCode() == toCompare.GetHashCode();
        }

        public override int GetHashCode()
        {
            var result = (Model == null ? 0 : Model.GetHashCode());
            result += 17*(SerialNumber == null ? 0 : SerialNumber.GetHashCode());
            return result;
        }
    }
}
