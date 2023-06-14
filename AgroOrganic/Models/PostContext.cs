using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models
{
    public class PostContext : DbContext
    {
        public PostContext() : base("DefaultConnection")
        {
        }

        public DbSet<Post> Post { get; set; }
    }
}