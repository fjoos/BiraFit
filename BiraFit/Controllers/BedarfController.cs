using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiraFit.Models;

namespace BiraFit.Controllers
{
    public class BedarfController : Controller
    {
        public static List<Bedarf> BedarfList;
        private ApplicationDbContext _context;

        public BedarfController()
        {
            _context = new ApplicationDbContext();
            /*
            BedarfList = new List<Bedarf>();
            BedarfList.Add(new Bedarf() { Id = 1, Open = true, Beschreibung = "Hallo Zusammen, Ich suche einen PersonalTrainer der mir hilft abzunehmen. Bin 24 Jahre alt und speiele gerne Fussball", Ort = "St. Gallen", Preis = 150 });
            BedarfList.Add(new Bedarf() { Id = 2, Open = false, Beschreibung = "Hallo Zusammen, Ich suche einen PersonalTrainer der mir hilft muskeln aufzubauen. Bin 34 Jahre alt und speiele gerne Tennis", Ort = "Rapperswil", Preis = 250 });
            */
        }

        protected override void Dispose(bool disposing)
        {
           _context.Dispose();
        }

        // GET: Bedarf
        public ActionResult Index()
        {
            var BedarfList = _context.Bedarf.ToList();
            return View(BedarfList);
        }

    }
}