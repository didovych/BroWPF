namespace BroData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contragent")]
    public partial class Contragent
    {
        public Contragent()
        {
            ContragentTransactions = new HashSet<Transaction>();
            OperatorTransactions = new HashSet<Transaction>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public virtual ICollection<Transaction> ContragentTransactions { get; set; }

        public virtual ICollection<Transaction> OperatorTransactions { get; set; }

        public virtual Client Client { get; set; }

        public virtual Repairer Repairer { get; set; }

        public virtual Salesman Salesman { get; set; }
    }
}
