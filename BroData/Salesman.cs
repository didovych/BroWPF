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

        public virtual Contragent Contragent { get; set; }
    }
}
