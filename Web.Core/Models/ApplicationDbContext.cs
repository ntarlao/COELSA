using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Core.Models
{
    public class ApplicationDbContext: DbContext
    {

        protected readonly IConfiguration Configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

            protected override void OnConfiguring(DbContextOptionsBuilder options) 
            {
                options.UseSqlServer(Configuration.GetConnectionString("WebDataBase")); 
                //Database.EnsureCreated();
            }
      
        public DbSet<Contacts> Contacts { get; set; }
    }
}
