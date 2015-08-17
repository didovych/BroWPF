namespace BroData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Repairer")]
    public partial class Repairer
    {
        public int ID { get; set; }

        public virtual Contragent Contragent { get; set; }
    }
}
