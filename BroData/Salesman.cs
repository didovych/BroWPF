using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace BroData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Salesman")]
    public partial class Salesman
    {
        public int ID { get; set; }

        public int ProfitPercentage { get; set; }

        public int SalaryPerDay { get; set; }

        [Required, MaxLength(50)]
        public string Login { get; set; }

        public virtual Contragent Contragent { get; set; }
    }
}
