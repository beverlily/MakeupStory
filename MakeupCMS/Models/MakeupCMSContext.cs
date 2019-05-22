using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MakeupCMS.Models
{ 
    //subclass of DbContext
    //DbContext = database connection + set of database tables, links model properties to database through connection string
    public class MakeupCMSContext : DbContext
    {
        public MakeupCMSContext()
        {

        }

        //tables in the database
        public DbSet<MakeupProduct> MakeupProducts { get; set; }
        public DbSet<Brand> Brands { get; set; }

    }
}