using System.Collections.Generic;
using Gruppeoppgave_1.Model;

namespace Gruppeoppgave_1.BLL
{
    public interface IBestillingLogikk
    {
        bool admin_i_db(Admin innAdmin); 
        List<Avganger> alleAvganger(int destFra, int destTil, int tid); 
        List<Avganger> alleReturAvganger(int destFra, int destTil, int tid); 
        bool endreAdmin(int id, string fornavn, string etternavn, string telefon, string epost); 
        bool endreBestilling(int id, string dato, string datoRetur, int antallReisende, int totalPris); 
        bool endreDestinasjoner(int id, string sted, int sone); 
        bool endrePris(int id, int prisVoksen, int prisStudent, int prisBarn, int prisUngdom, int prisHonnor, int prisVerneplikt, int prisPerSone); 
        List<RegistrerAdmin> getAdmin(); 
        List<Bestilling> getBestillinger(); 
        List<Priser> getPris();  
        List<Destinasjoner> hentDestinasjon();  
        string hentPriser(); 
        Avganger hentValgtAvgang(int destFra, int destTil); 
        List<Destinasjoner> leggInnDestinasjon(string sted, int sone); 
        bool registrerAdmin(RegistrerAdmin innAdmin); 
        bool settInnAvganger(); 
        bool settInnBestillingEnvei(int avgangId, string dato, int antallReisende, int totalSum); 
        bool settInnBestillingTurRetur(int avgangId, int returId, string dato, string datoRetur, int antallReisende, int totalSum); 
        bool settInnDestinasjon(); 
        bool settInnPris(); 
        bool slettAdmin(int id); 
        bool slettBestilling(int id); 
        bool slettDestinasjon(int id); 
        bool endreAvgang(int id, int destFra, int destTil, string avgang, string ankomst); 
        List<EndringsLoggModel> hentLogg(); 
        bool slettLogg(int id); 
        bool slettAvgang(int id, int destFra, int destTil); 
    }
}