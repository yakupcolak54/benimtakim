using dükkan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace dükkan.Controllers
{
    public class HomeController : Controller
    {
        Db db = new Db();
        public ActionResult anasayfa()
        {
            var anaSayfaModel = new AnaSayfaModel();
            anaSayfaModel.takilar = db.takilar.ToList();
            anaSayfaModel.kategorilar = db.kategoriler.ToList();
            return View(anaSayfaModel);
        }
        [HttpPost]
        public ActionResult anasayfa(string check,string kategori)
        {
            var takilar = new List<Taki>();

            if (check != null && kategori != null)
            {
                if (kategori == "-1")
                    takilar = db.takilar.ToList();
                else {
                    int id;
                    if (int.TryParse(kategori, out id))
                    {
                        takilar = db.takilar.Where(x => x.kategoriId == id).ToList();
                    }
                }


                if (check=="0")
                    takilar =takilar.OrderBy(x => x.takiAdi).ToList();
                else if (check=="1")
                    takilar =takilar.OrderByDescending(x => x.takiAdi).ToList();
                else if (check == "2")
                    takilar = takilar.OrderByDescending(x => x.fiyat).ToList();
                else if (check == "3")
                    takilar = takilar.OrderBy(x => x.fiyat).ToList();
            }
            var anaSayfaModel = new AnaSayfaModel();
            anaSayfaModel.takilar = takilar;
            anaSayfaModel.kategorilar = db.kategoriler.ToList();
            return View(anaSayfaModel);
        }
        public ActionResult admin()
        {
            return View();
        }
        public ActionResult hakkimizda()
        {
            return View();
        }
        public ActionResult admingiris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult admingiris(string password,string userName)
        {
            var adminUserInfo = db.adminler.FirstOrDefault(x => x.userName == userName && x.password == password);
            if (adminUserInfo != null)
            {
                FormsAuthentication.SetAuthCookie(adminUserInfo.userName, false);
                Session["username"]=adminUserInfo.userName;
                return RedirectToAction("anasayfa","home");
            }
            return View();
        }
        public ActionResult urunsayfa()
        {
            return View();
        }
        public ActionResult incele(string id) 
        {
            if (id != null)
            {
                var taki = db.takilar.FirstOrDefault(x => x.id.ToString() == id);
                if (taki != null)
                {
                    return View(taki);
                }
            }

            return Redirect("/home/anasayfa");
        }
        


    }
}