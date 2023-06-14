namespace AgroOrganic.Models.ModelContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelContext : DbContext
    {
        public ModelContext()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<AdviceToGrowForElement> AdviceToGrowForElements { get; set; }
        public virtual DbSet<Plant> Plants { get; set; }
        public virtual DbSet<Ranx> Ranges { get; set; }
        public virtual DbSet<SymptomDescription> SymptomDescriptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plant>()
                .HasMany(e => e.AdviceToGrowForElements)
                .WithRequired(e => e.Plant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ranx>()
                .Property(e => e.RangeName)
                .IsUnicode(false);
        }
    }
}
