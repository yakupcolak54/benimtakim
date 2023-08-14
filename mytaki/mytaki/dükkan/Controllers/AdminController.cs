using dükkan.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using ImageResizer;
using ImageMagick;

namespace dükkan.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        Db db = new Db();

        public ActionResult kategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult kategoriEkle(string ad)
        {
            if (ad != null)
            {
                var kategori = new Kategori();
                kategori.kategoriAdi = ad;
                db.kategoriler.Add(kategori);
                db.SaveChanges();
            }
            return View();
        }
        [HttpPost]
        public ActionResult takiEkle(string ad, string ozellik, string kategoriId, HttpPostedFileBase file1, HttpPostedFileBase file2, string fiyat)
        {
            if (ad != null && ozellik != null && kategoriId != null)
            {
                string yol1 = "/images/taki/" + file1.FileName;
                string yol2 = "/images/taki/" + file2.FileName;

                file1.SaveAs(Server.MapPath(yol1));
                file2.SaveAs(Server.MapPath(yol2));

                string inputPath = Server.MapPath(yol1);
                string outputPath = Server.MapPath(yol1);

                using (MagickImage image = new MagickImage(inputPath))
                {
                    // Yeni genişlik ve yükseklik değerleri

                    int newWidth = 500;
                    int newHeight = 500;

                    MagickGeometry geo = new MagickGeometry();
                    geo.Width = newWidth;
                    geo.Height = newHeight;

                    image.Chop(geo);
                    image.Write(outputPath);
                }

                var taki = new Taki();
                taki.file1 = yol1;
                taki.file2 = yol2;
                taki.fiyat = float.Parse(fiyat);
                taki.takiAdi = ad;
                taki.ozellikler = ozellik;
                taki.kategoriId = int.Parse(kategoriId);
                db.takilar.Add(taki);
                db.SaveChanges();
            }
            return View(db.kategoriler.ToList());
        }
        public ActionResult takiEkle()
        {
            return View(db.kategoriler.ToList());
        }
        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("anasayfa","home");
        }
        public ActionResult Kategoriler()
        {

            return View(db.kategoriler.ToList());
        }
        public ActionResult kategoriSil(string id)
        {
            var deger = db.kategoriler.FirstOrDefault(x => x.id.ToString() == id);
            if (deger != null)
            {
                db.kategoriler.Remove(deger);
                db.SaveChanges();
            }
            return Redirect("/admin/kategoriler");

        }
        public ActionResult takisil(string id)
        {
            var deger = db.takilar.FirstOrDefault(x => x.id.ToString() == id);
            if (deger != null)
            {
                db.takilar.Remove(deger);
                db.SaveChanges();
            }
            return Redirect("/home/anasayfa");

        }
        public ActionResult takiguncelle(string id)
        {
            var deger = db.takilar.FirstOrDefault(x => x.id.ToString() == id);
            if (deger != null)
            {
                var takiguncelleModel = new TakiGuncelleModel();
                takiguncelleModel.taki = deger;
                takiguncelleModel.kategoriler = db.kategoriler.ToList();
                return View(takiguncelleModel);
            }
            return Redirect("/home/anasayfa");
        }
        [HttpPost]
        public ActionResult takiguncelle(string ad, string ozellik, string kategoriId, string fiyat, HttpPostedFileBase file1, HttpPostedFileBase file2,string id)
        {
            var deger = db.takilar.FirstOrDefault(x => x.id.ToString() == id);
            if (deger != null)
            {
                deger.ozellikler = ozellik;
                deger.takiAdi = ad;
                deger.kategoriId = int.Parse(kategoriId);
                
                string yol2 = "/images/taki/" + file2.FileName;
                string yol1 = "/images/taki/" + file1.FileName;/*
                var Image = ConvertToImage(file1);
                var Image1 = ConvertToImage(file2);
                var Image2 = ResizeImage(Image,400,400);
                var Image3 = ResizeImage(Image, 400, 400);

                Image2.Save(Server.MapPath(yol1));
                Image3.Save(Server.MapPath(yol2));*/



                file1.SaveAs(Server.MapPath(yol1));
                file2.SaveAs(Server.MapPath(yol2));
                ResizeSettings resizeSetting = new ResizeSettings("maxwidth=100&maxheight=100");

                ImageBuilder.Current.Build(yol1, yol1, resizeSetting);
                ImageBuilder.Current.Build(yol2, yol2, resizeSetting);
                deger.fiyat = float.Parse(fiyat);
                db.SaveChanges();
            }
            return Redirect("/home/anasayfa");
        }
        public Image ResizeImage(Image img, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(img, destRect, 0, 0, width, height, GraphicsUnit.Pixel);
            }

            return destImage;
        }
        public Image ConvertToImage(HttpPostedFileBase file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                return Image.FromStream(memoryStream);
            }
        }


    }
}