namespace htcustomer.entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ToDo")]
    public partial class ToDo
    {
        public int ToDoID { get; set; }

        public string Content { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        public DateTime? RemindTime { get; set; }

        public int? Priority { get; set; }

        public bool? Disable { get; set; }
    }
}
