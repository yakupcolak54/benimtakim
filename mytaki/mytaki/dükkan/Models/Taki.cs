using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dükkan.Models
{
    public class Taki
    {
        public int id { get; set; }
        public int kategoriId { get; set; }

        public float fiyat { get; set; }
        public string takiAdi { get; set; }
        public string ozellikler { get; set; }

        public string file1 { get; set; }
        public string file2 { get; set; }


    }
}