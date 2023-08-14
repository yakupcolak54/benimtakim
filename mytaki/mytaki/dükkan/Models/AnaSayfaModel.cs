using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dükkan.Models
{
    public class AnaSayfaModel
    {
        public List<Taki> takilar { get; set; }
        public List<Kategori> kategorilar { get; set; }
    }
}