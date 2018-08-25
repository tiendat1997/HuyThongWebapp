namespace htcustomer.entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetailPrice")]
    public partial class DetailPrice
    {
        public int DetailPriceID { get; set; }

        public int? TransactionID { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public double? Price { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
