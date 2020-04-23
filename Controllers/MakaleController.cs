using mvcBlog.Helper;
using mvcBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvcBlog.Controllers
{
    public class MakaleController : YetkiliController
    {
        Blog db = new Blog();
        // GET: Makale
        public ActionResult Index()
        {
            var makaleler = db.Makales.ToList();

            return View(makaleler);
        }

        // GET: Makale/Details/5
        public ActionResult Details(int id)
        {
            var makale = db.Makales.Where(i => i.id == id).SingleOrDefault();
            return View(makale);
        }
        public ActionResult KisiMakaleListele()
        {
            var kullaniciAdi = Session["username"].ToString();
            var makaleler = db.Kullanicis.Where(a => a.kullaniciAdi == kullaniciAdi).SingleOrDefault().Makales.ToList();


            return View(makaleler);
        }

        // GET: Makale/Create
        public ActionResult Create()
        {

            #region MyRegion
            //ViewBag.kategoriAdi = db.Kategorilers.ToList();*///new SelectList(db.Kategorilers, "kategoriId", "kategoriAdi");
            #endregion

            ViewBag.kategoriId = new SelectList(db.Kategorilers, "kategoriId", "kategoriAdi");
            return View();

        }

        // POST: Makale/Create
        [HttpPost]
        public ActionResult Create(Makale model)
        {
            try
            {
                string kullaniciAdi = Session["username"].ToString();
                var kullanici = db.Kullanicis.Where(i => i.kullaniciAdi == kullaniciAdi).SingleOrDefault();
                model.kullaniciId = kullanici.id;
                model.tarih = DateTime.Now;
                db.Makales.Add(model);
                db.SaveChanges();

                return RedirectToAction("Index", "Kullanici");


            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

            #region MyRegion
            //try
            //{
            //    ViewBag.kategoriAdi = db.Kategorilers.ToList();
            //    string kullaniciAdi = Session["username"].ToString();
            //    var kullanici = db.Kullanicis.Where(i => i.kullaniciAdi == kullaniciAdi).SingleOrDefault();
            //    model.kullaniciId = kullanici.id;
            //    model.kategoriId = KategoriListId;
            //    model.tarih = DateTime.Now;
            //    db.Makales.Add(model);
            //    db.SaveChanges();


            //    return RedirectToAction("Index","Kullanici");
            //}
            //catch(Exception ex)
            //{
            //    return View(ex.Message);
            //} 
            #endregion
        }

        // GET: Makale/Edit/5
        public ActionResult Edit(int id)
        {
            string kullaniciAdi = Session["username"].ToString();
            var makale = db.Makales.Where(i => i.id == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            if (makale.Kullanici.kullaniciAdi == kullaniciAdi)
            {
                ViewBag.kategoriId = new SelectList(db.Kategorilers, "kategoriId", "kategoriAdi");
                return View(makale);
            }
            return HttpNotFound();
        }

        // POST: Makale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Makale model)
        {
            try
            {
                var makale = db.Makales.Where(i => i.id == id).SingleOrDefault();
                makale.Baslik = model.Baslik;
                makale.Icerik = model.Icerik;
                makale.kategoriId = model.kategoriId;
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                var kullaniciAdi = Session["username"].ToString();
                var kullanici = db.Kullanicis.Where(a => a.kullaniciAdi == kullaniciAdi).SingleOrDefault();
                var makale = db.Makales.Where(i => i.id == id).SingleOrDefault();

                if (OrtakSinif.DeleteIsimYetkiVarMi(id, kullanici))
                {
                    //makale.kullanici==null;
                    foreach (var item in makale.Yorums)
                    {
                        db.Yorums.Remove(item);
                    }
                    foreach (var item in makale.Etikets)
                    {
                        db.Etikets.Remove(item);
                    }

                    return RedirectToAction("Index");
                }
                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Makale Silinemedi." });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Hata", "Yetkili", new
                {
                    yazilacak = "Makale Silinemedi." + " Hata:"
                + ex.Message
                });
            }
        }
        public ActionResult YorumListele()
        {
            return PartialView();
        }

        public JsonResult YorumYap(string yorum, int makaleId)
        {
            var kullaniciAdi = Session["username"].ToString();
            var kullanici = db.Kullanicis.Where(i => i.kullaniciAdi == kullaniciAdi).SingleOrDefault();

         
            try
            {
                db.Yorums.Add(new Yorum { kullaniciId = kullanici.id, makaleId = makaleId, tarih = DateTime.Now, YorumIcerik = yorum });
                db.SaveChanges();

                return Json(false, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult YorumDelete(int id)
        {
            var kullaniciAdi = Session["username"].ToString();
            var kullanici = db.Kullanicis.Where(a => a.kullaniciAdi == kullaniciAdi).SingleOrDefault();
            var yorum = db.Yorums.Where(i => i.id == id).SingleOrDefault();
            var makale = db.Makales.Where(i => i.id == id).SingleOrDefault();

            if (yorum == null)
            {
                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Yorum Bulunamadı." });
            }
            if (OrtakSinif.DeleteIsimYetkiVarMi(id, kullanici) || makale.kullaniciId == kullanici.id)
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();

                return RedirectToAction("Details", "Makale", new { id = yorum.makaleId });

            }

            return View("Hata", "Yetkili", new { yazilacak = "Yorum Silinemedi." });
        }


    }
}
