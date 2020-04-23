namespace mvcBlog.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Blog : DbContext
    {
        public Blog()
            : base("name=Blog")
        {
        }

        public virtual DbSet<Etiket> Etikets { get; set; }
        public virtual DbSet<Kategoriler> Kategorilers { get; set; }
        public virtual DbSet<Kullanici> Kullanicis { get; set; }
        public virtual DbSet<Makale> Makales { get; set; }
        public virtual DbSet<Yetki> Yetkis { get; set; }
        public virtual DbSet<Yorum> Yorums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Etiket>()
                .HasMany(e => e.Makales)
                .WithMany(e => e.Etikets)
                .Map(m => m.ToTable("Etiket-Makale").MapLeftKey("etiketId").MapRightKey("makaleId"));

            modelBuilder.Entity<Kategoriler>()
                .HasMany(e => e.Makales)
                .WithRequired(e => e.Kategoriler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Makales)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Yorums)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Makale>()
                .HasMany(e => e.Yorums)
                .WithRequired(e => e.Makale)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Yetki>()
                .HasMany(e => e.Kullanicis)
                .WithRequired(e => e.Yetki)
                .WillCascadeOnDelete(false);
        }
    }
}
