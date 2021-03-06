namespace htcustomer.entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            this.ProductHistories = new HashSet<ProductHistory>();
        }

        [Key]
        public int ProductID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Amount { get; set; }

        [StringLength(50)]
        public string Seller { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        public double? BuyPrice { get; set; }

        public double? SellPrice { get; set; }

        public DateTime? BuyDate { get; set; }

        public int? Type { get; set; }

        public bool? Disable { get; set; }

        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<ProductHistory> ProductHistories { get; set; }
    }
}
