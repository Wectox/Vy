using Gruppeoppgave_1.DAL;
using Gruppeoppgave_1.Model;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Gruppeoppgave_1.BLL
{
    public class BestillingLogikk : IBestillingLogikk
    {
        private IBestillingRepository _repository;

        public BestillingLogikk()
        {
            _repository = new BestillingRepository();
        }

        public BestillingLogikk(IBestillingRepository stub)
        {
            _repository = stub;
        }

        public List<Avganger> alleAvganger(int destFra, int destTil, int tid)
        {
            List<Avganger> alleAvganger = _repository.alleAvganger(destFra, destTil, tid);
            return alleAvganger;
        }

        public List<Avganger> alleReturAvganger(int destFra, int destTil, int tid)
        {
            List<Avganger> alleAvganger = _repository.alleReturAvganger(destFra, destTil, tid);
            return alleAvganger;
        }

        public Avganger hentValgtAvgang(int destFra, int destTil)
        {
            return _repository.hentValgtAvgang(destFra, destTil);
        }

        public string hentPriser()
        {
            var BestillingDAL = new BestillingRepository();
            var priser = BestillingDAL.hentPriser();
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(priser);
            return json;
        }


        public bool settInnBestillingEnvei(int avgangId, string dato, int antallReisende, int totalSum)
        {
            return _repository.settInnBestillingEnvei(avgangId, dato, antallReisende, totalSum);
        }

        public bool settInnBestillingTurRetur(int avgangId, int returId, string dato, string datoRetur, int antallReisende, int totalSum)
        {
            return _repository.settInnBestillingTurRetur(avgangId, returId, dato, datoRetur, antallReisende, totalSum);
        }


        public List<Destinasjoner> hentDestinasjon()
        {
            return _repository.hentDestinasjon();
        }

        public bool endreDestinasjoner(int id, string sted, int sone)
        {
            return _repository.endreDestinasjoner(id, sted, sone);
        }

        public bool slettDestinasjon(int id)
        {
            return _repository.slettDestinasjon(id);
        }

        public List<Destinasjoner> leggInnDestinasjon(string sted, int sone)
        {
            return _repository.leggInnDestinasjon(sted, sone);          
        }

        public List<Bestilling> getBestillinger()
        {
            return _repository.getBestillinger();
        }

        public bool slettBestilling(int id)
        {
            return _repository.slettBestilling(id);
        }

        public bool endreBestilling(int id, string dato, string datoRetur, int antallReisende, int totalPris)
        {
            return _repository.endreBestilling(id, dato, datoRetur, antallReisende, totalPris);
        }

        public List<RegistrerAdmin> getAdmin()
        {
          
            List<RegistrerAdmin> admin = _repository.getAdmin();
            return admin;
        }

   
        public bool slettAdmin(int id)   
        {
            return _repository.slettAdmin(id);
        }

        public bool endreAdmin(int id, string fornavn, string etternavn, string telefon, string epost)
        {
            return _repository.endreAdmin(id, fornavn, etternavn, telefon, epost);
        }


        public bool admin_i_db(Admin innAdmin)
        {
            return _repository.admin_i_db(innAdmin);
        }

        public bool registrerAdmin(RegistrerAdmin innAdmin)
        {
            return _repository.registrerAdmin(innAdmin);
        }


        public List<Priser> getPris()
        {
            return _repository.getPris();
        }

        public bool endrePris(int id, int prisVoksen, int prisStudent, int prisBarn, int prisUngdom, int prisHonnor, int prisVerneplikt, int prisPerSone)
        {
            return _repository.endrePris(id, prisVoksen, prisStudent, prisBarn, prisUngdom, prisHonnor, prisVerneplikt, prisPerSone);
        }

        public bool endreAvgang(int id, int destFra, int destTil, string avgang, string ankomst)
        {
            return _repository.endreAvgang(id, destFra, destTil, avgang, ankomst);
        }

        public bool slettAvgang(int id, int destFra, int destTil)
        {
            return _repository.slettAvgang(id, destFra, destTil);
        }

        public List<EndringsLoggModel> hentLogg()
        {
            return _repository.hentLogg();
        }

        public bool slettLogg(int id)
        {
            return _repository.slettLogg(id);
        }

        public bool settInnDestinasjon()
        {
            return _repository.settInnDestinasjon();
        }

        public bool settInnAvganger()
        {
            return _repository.settInnAvganger();
        }

        public bool settInnPris()
        {
            return _repository.settInnPris();
        }
    }
}