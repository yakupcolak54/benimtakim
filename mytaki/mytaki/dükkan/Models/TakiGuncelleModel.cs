using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dükkan.Models
{
    public class TakiGuncelleModel
    {
        public Taki taki { get; set; }
        public List<Kategori> kategoriler{ get; set; }
    }
}