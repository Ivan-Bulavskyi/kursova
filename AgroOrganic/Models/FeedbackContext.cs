using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models
{
    public class FeedbackContext : DbContext
    {
        public FeedbackContext() : base("DefaultConnection")
        {
        }

        public DbSet<Feedback> Feedback { get; set; }

    }
}