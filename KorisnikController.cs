using Da_ne_bude_da_nije_video.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Da_ne_bude_da_nije_video.Controllers
{
    [Filters.AutorizeAdmin]
    public class KorisnikController : Controller
    {
        private VideoContext _context;

        public KorisnikController()
        {
            _context = new VideoContext();
        }
        //vraća sve korisnike
        public ActionResult Index(string pretraga)
        {
            return View(_context.Korisnik.Where(x => x.KorisnickoIme.Contains(pretraga) || pretraga == null).ToList());
        }

        
        public ActionResult DodajKorisnika()
        {
            var korisnik = new KorisnikViewModel();
            return View(korisnik);
        }

        //Dodaje prosledjenog korisnika u bazu
        [HttpPost]
        public ActionResult DodajKorisnika(KorisnikViewModel korisnik)
        {
            if (ModelState.IsValid)
            {
                var noviKorisnik = new Korisnik
                {
                    ID = korisnik.ID,
                    Ime = korisnik.Ime,
                    Prezime = korisnik.Prezime,
                    KorisnickoIme = korisnik.KorisnickoIme,
                    Lozinka = korisnik.Lozinka,
                    PravoPristupa = korisnik.PravoPristupa,
                };

                _context.Korisnik.Add(noviKorisnik);
                _context.SaveChanges();
                return RedirectToAction("Index", "Korisnik");
            }

            return View(korisnik);

        }

        public ActionResult Izbrisi(long? id)
        {
            return View(PronadjiKorisnika(id));
        }

        //Brise prosledjenog korisnika iz baze
        [HttpPost]
        public ActionResult Izbrisi(long? id, FormCollection fcNotUsed)
        {
            VideoContext _context;
            _context = new VideoContext();

            var korisnik = _context.Korisnik.Where(x => x.ID == id).FirstOrDefault();
            _context.Korisnik.Remove(korisnik);
            _context.SaveChanges();

            return RedirectToAction("Index", "Film");
        }

        //Metoda koja nam pomaze da pronadjemo prosledjenog korisnika u bazi
        public Korisnik PronadjiKorisnika(long? id)
        {
            Korisnik korisnik = new Korisnik();
            korisnik = _context.Korisnik.Where(x => x.ID == id).FirstOrDefault();
            return korisnik;
        }
        
        public ActionResult Izmeni(long? id)
        {
            return View(PronadjiKorisnika(id));
        }

        //Vrsi izmene prosledjenog korisnika u bazi
        [HttpPost]
        public ActionResult Izmeni(KorisnikViewModel korisnik, long? id)
        {
            IzmeniKorisnika(korisnik, id);
            return RedirectToAction("Index", "Korisnik");
        }

        //Metoda koja nam pomoze da izmenimo korisnika, koju prosledjujemo ActionResult-u
        public ActionResult IzmeniKorisnika(KorisnikViewModel korisnik, long? id)
        {
            var k = _context.Korisnik.Find(id);
            if (k == null)
            {
                return new HttpNotFoundResult();
            }

            k.Ime = korisnik.Ime;
            k.Prezime = korisnik.Prezime;
            k.KorisnickoIme = korisnik.KorisnickoIme;
            k.Lozinka = korisnik.Lozinka;
            k.PravoPristupa = korisnik.PravoPristupa;

            _context.Entry(k).State = EntityState.Modified;
            _context.SaveChanges();
            return View(korisnik);

        }
        //Vraća detalje o korisniku
        [HttpGet]
        public ActionResult DetaljiOKorisniku(long id)
        {
            return View(PronadjiKorisnika(id));

        }

    }
    
}
