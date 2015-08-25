namespace BroData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transaction")]
    public partial class Transaction
    {
        public int ID { get; set; }

        public int? ProductID { get; set; }

        public DateTime Date { get; set; }

        public int TypeID { get; set; }

        public int? ContragentID { get; set; }

        public int OperatorID { get; set; }

        public decimal? Price { get; set; }

        public virtual Contragent Contragent { get; set; }

        public virtual Contragent Operator { get; set; }

        public virtual Product Product { get; set; }

        [StringLength(50)]
        public string Notes { get; set; }

        public virtual TransactionType TransactionType { get; set; }
    }
}
