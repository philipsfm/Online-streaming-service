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
    public class FilmController : Controller
    {
        private VideoContext _context;

        public FilmController()
        {
            _context = new VideoContext();
        }
        
        public ActionResult Index(string pretraga)
        {
            return View(_context.Film.Where(x => x.Naziv.Contains(pretraga) || pretraga == null).ToList());
        }

        //Ispisuje podatke o trazenom filmu
        [HttpGet]
        public ActionResult DetaljiOFilmu(long id)
        {
            return View(PronadjiFilm(id));

        }
        
        public ActionResult DodajFilm()
        {
            var film = new FilmViewModel();
            return View(film);
        }

        //Dodaje prosledjen film u bazu
        [HttpPost]
        public ActionResult DodajFilm(FilmViewModel film, HttpPostedFileBase videofile)
        {

            var validImageTypes = new string[]
        {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
        };

            if (film.ImageUpload == null || film.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "Polje je obavezno");
            }
            else if (!validImageTypes.Contains(film.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Molimo odaberite podatak sa ovim ekstenzijama: GIF, JPG ili PNG.");
            }

            if (ModelState.IsValid)
            {
                if (videofile != null)
                {
                    string filename = Path.GetFileName(videofile.FileName);
                    if (videofile.ContentLength < 2147483647)
                    {
                        videofile.SaveAs(Server.MapPath("/Videofiles/" + filename));
                    }

                    var noviFilm = new Film
                    {
                        ID = film.ID,
                        Naziv = film.Naziv,
                        Godina = film.Godina,
                        Zanr = film.Zanr.ToString(),
                        Ocena = film.Ocena,
                        Trajanje = film.Trajanje,
                        Video = "/Videofiles/" + filename
                    };

                    if (film.ImageUpload != null && film.ImageUpload.ContentLength > 0)
                    {
                        var uploadDir = "~/Slike";
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), film.ImageUpload.FileName);
                        var imageUrl = Path.Combine(uploadDir, film.ImageUpload.FileName);
                        film.ImageUpload.SaveAs(imagePath);
                        noviFilm.Slika = imageUrl;
                    }

                    _context.Film.Add(noviFilm);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Film");
                }
            }
            return View(film);

        }
        
        public ActionResult Izbrisi(long? id)
        {
            return View(PronadjiFilm(id));
        }

        //Brise prosledjen film iz baze
        [HttpPost]
        public ActionResult Izbrisi(long? id, FormCollection fcNotUsed)
        {
            VideoContext _context;
            _context = new VideoContext();

            var film = _context.Film.Where(x => x.ID == id).FirstOrDefault();
            _context.Film.Remove(film);
            _context.SaveChanges();

            return RedirectToAction("Index", "Film");
        }

        //Metoda koja nam pomaze da pronadjemo prosledjen film u bazi
        public Film PronadjiFilm(long? id)
        {
            Film film = new Film();
            film = _context.Film.Where(x => x.ID == id).FirstOrDefault();
            return film;
        }
        
        public ActionResult Izmeni(long? id)
        {
            return View(PronadjiFilm(id));
        }

        //Vrsi izmene prosledjen film u bazi
        [HttpPost]
        public ActionResult Izmeni(FilmViewModel film, long? id, HttpPostedFileBase videofile)
        {
            IzmeniFilm(film, id, videofile);
            return RedirectToAction("Index", "Film");
        }

        //Metoda koja nam pomoze da izmenimo film, koju prosledjujemo ActionResult-u

        public ActionResult IzmeniFilm(FilmViewModel film, long? id, HttpPostedFileBase videofile)
        {
            var validImageTypes = new string[]
            {
                  "image/gif",
                  "image/gif",
                  "image/jpeg",
                  "image/pjpeg",
                  "image/png"
            };
            if (film.ImageUpload == null || film.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "Polje je obavezno");
            }
            else if (!validImageTypes.Contains(film.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Molimo odaberite podatak sa ovim ekstenzijama: GIF, JPG ili PNG.");
            }
           

                var filmovi = _context.Film.Find(id);
                if (filmovi == null)
                {
                    return new HttpNotFoundResult();
                }

            filmovi.Naziv = film.Naziv;
            filmovi.Zanr = film.Zanr.ToString();
            filmovi.Ocena = film.Ocena;
            filmovi.Godina = film.Godina;
            filmovi.Trajanje = film.Trajanje;


                if (film.ImageUpload != null && film.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/Slike";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), film.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, film.ImageUpload.FileName);
                    film.ImageUpload.SaveAs(imagePath);
                    filmovi.Slika = imageUrl;
                }
                if (videofile != null)
                {
                string filename = Path.GetFileName(videofile.FileName);
                if (videofile.ContentLength < 2147483647)
                    {
                        videofile.SaveAs(Server.MapPath("/Videofiles/" + videofile.FileName));
                    }

                    filmovi.Video = "/Videofiles/" + videofile.FileName;
                }
                _context.Entry(filmovi).State = EntityState.Modified;
                _context.SaveChanges();
                return View(film);
            
            
        }
    }
}


