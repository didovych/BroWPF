using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroData
{
    public enum TranType
    {
        Zero = 1,
        Sold = 2,
        Bought = 3,
        ToRepair = 4,
        OnRepair = 5,
        Repaired = 6,
        ToPawn = 8,
        Cashin = 9,
        Cashout = 10
    }

    [Table("TransactionType")]
    public partial class TransactionType
    {
        public TransactionType()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
