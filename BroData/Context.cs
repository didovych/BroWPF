namespace BroData
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Contragent> Contragents { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Repairer> Repairers { get; set; }
        public virtual DbSet<Salesman> Salesmen { get; set; }
        public virtual DbSet<Guard> Guards { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<TransactionType>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.TransactionType)
                .HasForeignKey(e => e.TypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Models)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contragent>()
                .HasMany(e => e.ContragentTransactions)
                .WithOptional(e => e.Contragent)
                .HasForeignKey(e => e.ContragentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contragent>()
                .HasMany(e => e.OperatorTransactions)
                .WithRequired(e => e.Operator)
                .HasForeignKey(e => e.OperatorID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contragent>()
                .HasOptional(e => e.Client)
                .WithRequired(e => e.Contragent);

            modelBuilder.Entity<Contragent>()
                .HasOptional(e => e.Repairer)
                .WithRequired(e => e.Contragent);

            modelBuilder.Entity<Contragent>()
                .HasOptional(e => e.Salesman)
                .WithRequired(e => e.Contragent);

            modelBuilder.Entity<Contragent>()
                .HasOptional(e => e.Guard)
                .WithRequired(e => e.Contragent);

            modelBuilder.Entity<Model>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Model)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.Product)
                .WillCascadeOnDelete(false);
        }
    }
}
