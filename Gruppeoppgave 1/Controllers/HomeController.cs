using Gruppeoppgave_1.Model;
using Gruppeoppgave_1.BLL;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Gruppeoppgave_1.Controllers
{
    public class HomeController : Controller
    {
        private IBestillingLogikk _bestillingBLL;

        public HomeController()
        {
            _bestillingBLL = new BestillingLogikk();
        }

        public HomeController(IBestillingLogikk stub)
        {
            _bestillingBLL = stub;
        }


        // GET: Home
        public ActionResult Index()
        {
            var bestillingDb = new BestillingLogikk();
            settInnDestinasjon();
            settInnAvganger();
            settInnPris();

            int destFra = 0;
            int destTil = 0;
            int tid = 0;
            List<Avganger> alleAvgang = bestillingDb.alleAvganger(destFra, destTil, tid);

            return View(alleAvgang);
        }

      
        public ActionResult registrerAdmin()
        {
            return View();
        }


        public ActionResult adminBestilling()
        {
            return View();
        }


        public ActionResult endreDestinasjon()
        {
            return View();
        }


        public ActionResult endrePris()
        {
            return View();
        }


        public ActionResult bekreftBestilling()
        {
            return View();
        }


        public ActionResult loggSide()
        {
            return View();
        }


        public string hentAvgangInfo(int destFra, int destTil)
        {
            Avganger valgAvgang = _bestillingBLL.hentValgtAvgang(destFra, destTil);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(valgAvgang);
            return json;
        }


        public string hentAvganger(int destFra, int destTil, int tid)
        {
            List<Avganger> alleAvgang = _bestillingBLL.alleAvganger(destFra, destTil, tid);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(alleAvgang);
            return json;
        }


        public string hentReturAvganger(int destFra, int destTil, int tid)
        {
            List<Avganger> alleAvgang = _bestillingBLL.alleReturAvganger(destFra, destTil, tid);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(alleAvgang);
            return json;
        }


        public string hentPriser()
        {
            var bll = new BestillingLogikk();
            return bll.hentPriser();
        }


        public string leggBestillingEnvei(int avgangId, string dato, int antallReisende, int totalSum)
        {
            bool OK = _bestillingBLL.settInnBestillingEnvei(avgangId, dato, antallReisende, totalSum);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(OK);
            return json;
        }


        public string leggBestillingTurRetur(int avgangId, int returId, string dato, string datoRetur, int antallReisende, int totalSum)
        {
            bool OK = _bestillingBLL.settInnBestillingTurRetur(avgangId, returId, dato, datoRetur, antallReisende, totalSum);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(OK);
            return json;
        }

      
        public ActionResult loggInn()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult loggInn(Admin innLogget)
        {
            if(admin_i_db(innLogget))
            {
                Session["LoggetInn"] = true;
                ViewBag.Innlogget = true;
                return RedirectToAction("AdminSide");
            }
            else
            {
                Session["LoggetInn"] = false;
                ViewBag.Innlogget = false;
                return View();
            }
        }

   
        public bool admin_i_db(Admin innAdmin)
        {
            var admin = _bestillingBLL.admin_i_db(innAdmin);
            return admin;
        }

   
        private static byte[] lagHash(string innPassord, byte[] innSalt)
        {
            const int keyLength = 24;
            var pbkdf2 = new Rfc2898DeriveBytes(innPassord, innSalt, 1000); 
            return pbkdf2.GetBytes(keyLength);
        }

  
        private static byte[] lagSalt()
        {
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csprng.GetBytes(salt);
            return salt;
        }

  
        public ActionResult AdminSide()
        {
            if (Session["LoggetInn"] != null)
            {
                bool loggetInn = (bool)Session["LoggetInn"];
                if (loggetInn)
                {
                    return View();
                }
            }
            return RedirectToAction("loggInn");
        }

      
        public ActionResult LoggUt()
        {
            Session["LoggetInn"] = false;
            return RedirectToAction("loggInn");
        }


        public ActionResult endreAvganger()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult registrerAdmin(RegistrerAdmin innAdmin)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_bestillingBLL.registrerAdmin(innAdmin))
            {
                ViewBag.nyAdmin = true;
                return RedirectToAction("loggInn");
            }
            else
            {
                ViewBag.nyAdmin = false;
                return View();
            }
        }


        public string getBestillinger()
        {
            List<Bestilling> bestillinger = _bestillingBLL.getBestillinger();
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(bestillinger);
            return json;
        }


        public string slettBestilling(int id)
        {
            bool slettOK = _bestillingBLL.slettBestilling(id);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(slettOK);
            return json;
        }


        public string endreBestilling(int id, string dato, string datoRetur, int antallReisende, int totalPris)
        {
            bool endreOK = _bestillingBLL.endreBestilling(id, dato, datoRetur, antallReisende, totalPris);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(endreOK);
            return json;
        }

        public string getAdmin()
        {
            List<RegistrerAdmin> admin = _bestillingBLL.getAdmin();
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(admin);
            return json;
        }

        public string slettAdmin(int id)
        {
            bool slettOK = _bestillingBLL.slettAdmin(id); 
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(slettOK);
            return json;
        }

        public string endreAdmin(int id, string fornavn, string etternavn, string telefon, string epost)
        {
            bool endreOK = _bestillingBLL.endreAdmin(id, fornavn, etternavn, telefon, epost);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(endreOK);
            return json;
        }


        public string hentDestinasjon()
        {
            List<Destinasjoner> destinasjoner = _bestillingBLL.hentDestinasjon();
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(destinasjoner);
            return json;
        }


        public string nyDestinasjon(string nySted, int nySone)
        {
            List<Destinasjoner> destinasjoner = _bestillingBLL.leggInnDestinasjon(nySted, nySone);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(destinasjoner);
            return json;
        }


        public string endreDestinasjoner(int id, string sted, int sone)
        {

            bool endreOK = _bestillingBLL.endreDestinasjoner(id, sted, sone);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(endreOK);
            return json;
        }


        public string slettDestinasjon(int id)
        {
            bool slettOK = _bestillingBLL.slettDestinasjon(id);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(slettOK);
            return json;
        }


        public string getPris()
        {
            List<Priser> priser = _bestillingBLL.getPris();
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(priser);
            return json;

        }


        public string endrePriser(int id, int prisVoksen, int prisStudent, int prisBarn, int prisUngdom, int prisHonnor, int prisVerneplikt, int prisPerSone)
        {
            bool endreOK = _bestillingBLL.endrePris(id, prisVoksen, prisStudent, prisBarn, prisUngdom, prisHonnor, prisVerneplikt, prisPerSone);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(endreOK);
            return json;
        }


        public string endreAvgang(int id, int destFra, int destTil, string avgang, string ankomst)
        {
            bool endreOK = _bestillingBLL.endreAvgang(id, destFra, destTil, avgang, ankomst);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(endreOK);
            return json;
        }


        public string slettAvgang(int id, int destFra, int destTil)
        {
            bool slettOK = _bestillingBLL.slettAvgang(id, destFra, destTil);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(slettOK);
            return json;
        }


        public string hentLogg()
        {
            List<EndringsLoggModel> alleLogg = _bestillingBLL.hentLogg();
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(alleLogg);
            return json;
        }


        public string slettLogg(int id)
        {
            bool slettLogg = _bestillingBLL.slettLogg(id);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(slettLogg);
            return json;
        }


        public bool settInnDestinasjon()
        {
            return _bestillingBLL.settInnDestinasjon();
        }


        public bool settInnAvganger()
        {
            return _bestillingBLL.settInnAvganger();
        }


        public bool settInnPris()
        {
            return _bestillingBLL.settInnPris();
        }
    }
}
