using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Da_ne_bude_da_nije_video.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Authorize(Da_ne_bude_da_nije_video.Korisnik korisnik)
        {
            using (VideoContext _context = new VideoContext())
            {
                var detalji = _context.Korisnik.Where(x => x.KorisnickoIme == korisnik.KorisnickoIme && x.Lozinka == korisnik.Lozinka).FirstOrDefault();
                if (detalji == null)
                {
                    TempData["msg"] = "<script>alert('Pogresna lozinka ili nepostojece korisnicko ime');</script>";
                    return View("Index", korisnik);
                }
                else
                {
                    Session["PravoPristupa"] = detalji.PravoPristupa;
                    Session["KorisnickoIme"] = detalji.KorisnickoIme;
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    
}
}