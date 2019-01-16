using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PumoxTest.Models;

namespace PumoxTest
{
    public class Context : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
        public Context() : base("name=AppContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Employee>()
                .HasRequired<Company>(s => s.company)
                .WithMany(g => g.Employees)
                .HasForeignKey<long>(s => s.CompanyId);

        }
    }
}

