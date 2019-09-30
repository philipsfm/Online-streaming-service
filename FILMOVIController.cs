using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Da_ne_bude_da_nije_video.Controllers
{
    public class FILMOVIController : Controller
    {
        private VideoContext _context;

        public FILMOVIController()
        {
            _context = new VideoContext();
        }

        public ActionResult Index(string pretraga)
        {
            return View(_context.Film.Where(x => x.Naziv.Contains(pretraga) || pretraga == null).ToList());
        }

        public ActionResult Detalji(long id)
        {
            return View(PronadjiFilm(id));
        }

        public Film PronadjiFilm(long? id)
        {
            Film film = new Film();
            film = _context.Film.Where(x => x.ID == id).FirstOrDefault();
            return film;
        }

        public ActionResult Komedije()
        {
            var filmovi = _context.Film.Where(x => x.Zanr.ToString() == "komedija").ToArray();
            return View(filmovi);
        }

        public ActionResult Akcije()
        {
            var filmovi = _context.Film.Where(x => x.Zanr.ToString() == "akcija").ToArray();
            return View(filmovi);
        }

        public ActionResult Horor()
        {
            var filmovi = _context.Film.Where(x => x.Zanr.ToString() == "horor").ToArray();
            return View(filmovi);
        }

        public ActionResult Ljubavni()
        { 
            var filmovi = _context.Film.Where(x => x.Zanr.ToString() == "ljubavni").ToArray();
            return View(filmovi);
        }
    }
}