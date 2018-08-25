namespace htcustomer.entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transaction")]
    public partial class Transaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transaction()
        {
            DetailPrices = new HashSet<DetailPrice>();
        }

        public int TransactionID { get; set; }

        public int? CustomerID { get; set; }

        public int? StatusID { get; set; }

        public double? Price { get; set; }

        public DateTime? RecievedDate { get; set; }

        public DateTime? DeliverDate { get; set; }

        public int? TypeID { get; set; }

        public string Error { get; set; }

        public bool? Delivered { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        public virtual Category Category { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailPrice> DetailPrices { get; set; }

        public virtual TransactionStatus TransactionStatus { get; set; }
    }
}
