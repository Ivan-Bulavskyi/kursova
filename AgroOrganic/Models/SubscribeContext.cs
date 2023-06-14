using System.Data.Entity;

namespace AgroOrganic.Models
{
    public class SubscribeContext : DbContext
    {
        public SubscribeContext() : base("DefaultConnection")
        {
        }

        public DbSet<Subscribe> Subscribe { get; set; }
    }
}