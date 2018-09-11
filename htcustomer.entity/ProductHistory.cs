using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htcustomer.entity
{
    [Table("ProductHistory")]
    public partial class ProductHistory
    {
        [Key]
        public int HistoryId { get; set; }
        public int? ProductId { get; set; }
        public int? Amount { get; set; }
        public DateTime? SellDate { get; set; }
        public double? SellPrice { get; set; }
        public virtual Product Product { get; set; }
    }
}
