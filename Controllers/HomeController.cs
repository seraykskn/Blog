using mvcBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvcBlog.Controllers
{
    public class HomeController : Controller
    {
        Blog db = new Blog();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Kullanici model)
        {
            try
            {
                var varMi = db.Kullanicis.Where(i => i.kullaniciAdi == model.kullaniciAdi).SingleOrDefault();
                if (varMi == null)
                {
                    return View();
                }
                if (varMi.Sifre == model.Sifre)
                {
                    Session["username"] = model.kullaniciAdi;
                    return RedirectToAction("Index","Kullanici");
                }
                else
                {
                    return View();
                }

            }

            catch
            {
                return View();
            }
        }

        // GET: Kullanici/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kullanici/Create
        [HttpPost]
        public ActionResult Create(Kullanici model)
        {
            try
            {
                var varMi = db.Kullanicis.Where(i => i.kullaniciAdi == model.kullaniciAdi).SingleOrDefault();
                if (varMi != null)
                {
                    return View();
                }
                if (string.IsNullOrEmpty(model.Sifre))
                {
                    return View();
                }
                model.kayitTarihi = DateTime.Now;
                model.yetkiId = 1;
                db.Kullanicis.Add(model);
                db.SaveChanges();
                Session["username"] = model.kullaniciAdi;


                return RedirectToAction("Index","Kullanici");
            }
            catch (Exception e)
            {
                string a = e.Message;
                return View();
            }
        }
    }
}