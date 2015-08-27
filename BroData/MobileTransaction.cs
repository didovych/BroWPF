using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroData
{
    [Table("MobileTransaction")]
    public partial class MobileTransaction
    {
        public int ID { get; set; }

        public decimal CreditSum { get; set; }

        public int MobileOperatorID { get; set; }

        public virtual MobileOperator MobileOperator { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
