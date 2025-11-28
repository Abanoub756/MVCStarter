using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class MvcDbContext : IdentityDbContext<ApplicationUser>
    {

        public MvcDbContext(DbContextOptions<MvcDbContext> options): base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=ABANOUB-LAPTOP\\SQLEXPRESS;Database=MvcDb;Trusted_Connection=True;TrustServerCertificate=True;");
        //}

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        }
}
