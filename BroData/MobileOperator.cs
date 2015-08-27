using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;

namespace BroData
{
    [Table("MobileOperator")]
    public partial class MobileOperator
    {
        public MobileOperator()
        {
            MobileTransactions = new HashSet<MobileTransaction>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<MobileTransaction> MobileTransactions { get; set; }
    }
}
