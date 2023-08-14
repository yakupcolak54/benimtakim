using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace dükkan.Models
{
    public class Db:DbContext
    {
        public DbSet<Kategori> kategoriler { get; set; }
        public DbSet<Taki> takilar { get; set; }
        public DbSet<Admin> adminler { get; set; }


    }
}