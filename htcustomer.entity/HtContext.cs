namespace htcustomer.entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HuythongDB : DbContext
    {
        public HuythongDB()
            : base("name=HtContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DetailPrice> DetailPrices { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductHistory> ProductHistories { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ToDo> ToDoes { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionStatus> TransactionStatus { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.TypeID);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()               
                .HasMany(e => e.ProductHistories)
                .WithOptional(e => e.Product)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<Product>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<ProductType>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.ProductType)
                .HasForeignKey(e => e.Type);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("UserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));
        }
    }
}
