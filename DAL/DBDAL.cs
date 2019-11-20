using Gruppeoppgave_1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NLog;

namespace Gruppeoppgave_1.DAL
{
    public class BestillingRepository : IBestillingRepository
    {
        Random random = new Random();
        Logger logger = LogManager.getCurrentClassLogger();

        public void settInnDestinasjon()
        {
            var db = new BestillingContext();
            List<Destinasjon> destinasjonerfra = db.destinasjon.ToList();
            if (destinasjonerfra.Count > 0) //Her sjekker koden om det er en tom destinasjonstabell, hvis den er tom så returnerer den og skriver ut
            {
                Console.Write("Tabellen er ikke tom");
                return;
            }

            var d1 = new Destinasjon();
            d1.sted = "Oslo";
            d1.sone = 1;

            var d2 = new Destinasjon();
            d2.sted = "Bergen";
            d2.sone = 5;

            var d3 = new Destinasjon();
            d3.sted = "Trondheim";
            d3.sone = 6;

            var d4 = new Destinasjon();
            d4.sted = "Ålesund";
            d4.sone = 4;

            var d5 = new Destinasjon();
            d5.sted = "Kristiansand";
            d5.sone = 5;

            var d6 = new Destinasjon();
            d6.sted = "Tromsø";
            d6.sone = 7;

            var d7 = new Destinasjon();
            d7.sted = "Bodø";
            d7.sone = 5;

            var d8 = new Destinasjon();
            d8.sted = "Fredrikstad";
            d8.sone = 1;

            var d9 = new Destinasjon();
            d9.sted = "Drammen";
            d9.sone = 2;

            var d10 = new Destinasjon();
            d10.sted = "Stavanger";
            d10.sone = 5;

            try
            {
                db.destinasjon.Add(d1);
                db.destinasjon.Add(d2);
                db.destinasjon.Add(d3);
                db.destinasjon.Add(d4);
                db.destinasjon.Add(d5);
                db.destinasjon.Add(d6);
                db.destinasjon.Add(d7);
                db.destinasjon.Add(d8);
                db.destinasjon.Add(d9);
                db.destinasjon.Add(d10);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Får ikke lagt inn verdiene i tabellen: " + e);
            }
        }

        public String fiksTid(int startTime, int startMinutter, int soneForskjell)
        {
            int timer = 0;
            int minutter = 0;
            soneForskjell *= 30;

            timer = soneForskjell / 60;
            minutter = soneForskjell % 60;

            timer += startTime;
            minutter += startMinutter;

            if(minutter / 60 > 0)
            {
                timer++;
                minutter = minutter % 60;
            }

            timer = timer % 24;

            if (timer < 10 && minutter < 10) return "0" + timer + ":0" + minutter;
            else if (timer < 10 && minutter >= 10) return "0" + timer + ":" + minutter;
            else if (timer >= 10 && minutter < 10) return timer + ":0" + minutter;
            else return timer + ":" + minutter;
        }


        public void settInnAvganger()
        {
            var db = new BestillingContext();
            List<Avgang> avganger = db.avgang.ToList();
            List<Destinasjon> destinasjoner = db.destinasjon.ToList();


            if (avganger.Count > 0) //Sjekker om avganger er tom
            {
                return;
            }

            for(int i = 1; i < destinasjoner.Count+1; i++)
            {
                for(int j = 1; j < destinasjoner.Count+1; j++)
                {
                    leggTilAvganger(i, j);
                }
            }

        }

        public List<Avganger> alleAvganger(int destFra, int destTil, int tid)
        {
            var db = new BestillingContext();
            System.Diagnostics.Debug.WriteLine("Tid: " + tid);
            List <Avgang> alleAvganger = db.avgang.Where(a => a.destinasjonFra_id == destFra && a.destinasjonTil_id == destTil && a.time >= tid).ToList();
            //System.Diagnostics.Debug.WriteLine("Antall avganger: " + alleAvganger.Count);
            List<Avganger> returnerAvganger = new List<Avganger>();

            foreach(Avgang avgang in alleAvganger)
            {
                var a = new Avganger()
                {
                    id = avgang.id,
                    destinasjonFra = avgang.destinasjonFra.sted,
                    destinasjonTil = avgang.destinasjonTil.sted,
                    tid = avgang.tid,
                    ankomst = avgang.ankomst
                };
                //System.Diagnostics.Debug.WriteLine("Fra: " + a.destinasjonFra + " Til: " + a.destinasjonTil);
                returnerAvganger.Add(a);
            }

            //System.Diagnostics.Debug.WriteLine("Antall returnerte avganger: " + returnerAvganger.Count);
            return returnerAvganger;

        }

        public List<Avganger> alleReturAvganger(int destFra, int destTil, int tid)
        {
            var db = new BestillingContext();
            System.Diagnostics.Debug.WriteLine("TidRetur: " + tid);
            List<Avgang> alleAvganger = db.avgang.Where(a => a.destinasjonFra_id == destTil && a.destinasjonTil_id == destFra && a.time >= tid).ToList();
            //System.Diagnostics.Debug.WriteLine("Antall avganger: " + alleAvganger.Count);
            List<Avganger> returnerAvganger = new List<Avganger>();

            foreach (Avgang avgang in alleAvganger)
            {
                var a = new Avganger()
                {
                    id = avgang.id,
                    destinasjonFra = avgang.destinasjonFra.sted,
                    destinasjonTil = avgang.destinasjonTil.sted,
                    tid = avgang.tid,
                    ankomst = avgang.ankomst
                };
                //System.Diagnostics.Debug.WriteLine("Fra: " + a.destinasjonFra + " Til: " + a.destinasjonTil);
                returnerAvganger.Add(a);
            }

            //System.Diagnostics.Debug.WriteLine("Antall returnerte avganger: " + returnerAvganger.Count);
            return returnerAvganger;

        }

        public Avganger hentValgtAvgang(int destFra, int destTil)
        {
            var db = new BestillingContext();
            Avgang valgtDBAvgang = db.avgang.Where(a => a.destinasjonFra_id == destFra && a.destinasjonTil_id == destTil).First();

            var valgtAvgang = new Avganger()
            {
                destinasjonFra = valgtDBAvgang.destinasjonFra.sted,
                destinasjonTil = valgtDBAvgang.destinasjonTil.sted,
                tid = valgtDBAvgang.tid,
                ankomst = valgtDBAvgang.ankomst
            };

            return valgtAvgang;
        }

//sjekk kobling HomeC->BLL->DAL
        public Pris hentPriser()
        {
            var db = new BestillingContext();
            Pris priser = db.pris.Where(p => p.id == 1).First();

            return priser;
        }

        public void settInnPris()
        {
            var db = new BestillingContext();
            List<Pris> priser = db.pris.ToList();
            if (priser.Count > 0) //Sjekker om pristabellen er tom
            {
                return;
            }

            var pris = new Pris();
            pris.id = 1;
            pris.prisVoksen = 60;
            pris.prisStudent = 45;
            pris.prisBarn = 30;
            pris.prisUngdom = 40;
            pris.prisHonnor = 40;
            pris.prisVerneplikt = 45;
            pris.prisPerSone = 23;

            try
            {
                db.pris.Add(pris);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Får ikke lagt inn verdier i tabellen" + e);
            }
        }

        public bool settInnBestillingEnvei(int avgangId, string dato, int antallReisende, int totalSum)
        {
            using (var db = new BestillingContext())
            {
                Avgang avgang = db.avgang.Where(a => a.id == avgangId).First();

                var nyBestilling = new Bestillinger();
                nyBestilling.enVei_id = avgangId;
                nyBestilling.dato = dato;
                nyBestilling.totalPris = totalSum;
                nyBestilling.antallReisende = antallReisende;
                nyBestilling.avgangEnVei = avgang;

                try
                {
                    db.bestillinger.Add(nyBestilling);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    throw new Exception("Får ikke lagt inn bestillingen: " + feil);
                }
            }
        }

        public bool settInnBestillingTurRetur (int avgangId, int returId, string dato, string datoRetur, int antallReisende, int totalSum)
        {
            using(var db = new BestillingContext())
            {
                Avgang avgang = db.avgang.Where(a => a.id == avgangId).First();
                Avgang retur = db.avgang.Where(a => a.id == returId).First();

                var nyBestilling = new Bestillinger();
                nyBestilling.enVei_id = avgangId;
                nyBestilling.turRetur_id = returId;
                nyBestilling.dato = dato;
                nyBestilling.datoRetur = datoRetur;
                nyBestilling.totalPris = totalSum;
                nyBestilling.antallReisende = antallReisende;
                nyBestilling.avgangEnVei = avgang;
                nyBestilling.avgangturRetur = retur;

                try
                {
                    db.bestillinger.Add(nyBestilling);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    throw new Exception("Får ikke lagt inn bestillingen: " + feil);
                }
            }
        }

        public List<Bestilling> alleBestillinger()
        {
            using(var db = new BestillingContext())
            {
                List<Bestilling> alleBestillinger = db.bestillinger.Select(b => new Bestilling
                {
                    id = b.id,
                    enVei = b.avgangEnVei.id,
                    turRetur = b.avgangturRetur.id,
                    dato = b.dato
                }).ToList();

                return alleBestillinger;
            }
        }

        public List<Destinasjoner> hentDestinasjon()
        {
            using(var db = new BestillingContext()) {
            List<Destinasjoner> destinasjon = db.destinasjon.Select(dest => new Destinasjoner
            {
                id = dest.id,
                sted = dest.sted,
                sone = dest.sone
            }).ToList();
            return destinasjon;
            }
        }

        public bool endreDestinasjoner(int id, string sted, int sone)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    var endreDestinasjon = db.destinasjon.Find(id);
                    endreDestinasjon.sted = sted;
                    endreDestinasjon.sone = sone;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    return false;
                }
            }
        }

        public bool slettDestinasjon(int id)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    Destinasjon slettDestinasjon = db.destinasjon.Where(a => a.id == id).First();
                    db.destinasjon.Remove(slettDestinasjon);

                    List<Avgang> avganger = db.avgang.Where(a => a.destinasjonFra_id == id || a.destinasjonTil_id == id).ToList();
                    foreach(Avgang a in avganger)
                    {
                        db.avgang.Remove(a);
                    }

                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    return false;

                }
            }
        }

        public List<Destinasjoner> leggInnDestinasjon(string sted, int sone)
        {
            using (var db = new BestillingContext())
            {
                var nyDestinasjon = new Destinasjon();
                nyDestinasjon.sted = sted;
                nyDestinasjon.sone = sone;

                try
                {
                    db.destinasjon.Add(nyDestinasjon);
                    db.SaveChanges();
                }
                catch (Exception feil)
                {
                    throw new Exception("Får ikke lagt inn destinasjonen: " + feil);
                }

                Destinasjon nyDest = db.destinasjon.OrderByDescending(d => d.id).FirstOrDefault(); //LastOrDefault funket ikke

                List < Destinasjoner > destinasjoner = db.destinasjon.Select(dest => new Destinasjoner
                {
                    id = dest.id,
                    sted = dest.sted,
                    sone = dest.sone
                }).ToList();

                foreach (Destinasjoner dest in destinasjoner)
                {
                    leggTilAvganger(dest.id, nyDest.id);
                    leggTilAvganger(nyDest.id, dest.id);
                }

                return destinasjoner;
            }
        }

        public List<Bestilling> getBestillinger()
        {
            using(var db = new BestillingContext())
            {
                List<Bestilling> bestillinger = db.bestillinger.Select(b => new Bestilling
                {
                    id = b.id,
                    enVei = b.enVei_id,
                    turRetur = b.turRetur_id,
                    dato = b.dato,
                    datoRetur = b.datoRetur,
                    antallReisende = b.antallReisende,
                    totalPris = b.totalPris
                }).ToList();
                return bestillinger;
            }
        }

        public bool slettBestilling(int id)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    Bestillinger slettBestilling = db.bestillinger.FirstOrDefault(b => b.id == id);
                    db.bestillinger.Remove(slettBestilling);
                    db.SaveChanges();
                    return true;
                }
                catch(Exception feil)
                {
                    return false;
                }
            }
        }

        public bool endreBestilling(int id, string dato, string datoRetur, int antallReisende, int totalPris)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    Bestillinger endreBestilling = db.bestillinger.Where(a => a.id == id).First();
                    endreBestilling.dato = dato;
                    endreBestilling.datoRetur = datoRetur;
                    endreBestilling.antallReisende = antallReisende;
                    endreBestilling.totalPris = totalPris;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    return false;
                }
            }
        }

        public List<RegistrerAdmin> getAdmin()
        {
            using(var db = new BestillingContext())
            {

                List<RegistrerAdmin> admin = db.admin.Select(a => new RegistrerAdmin
                {
                    id = a.id,
                    Fornavn = a.fornavn,
                    Etternavn = a.etternavn,
                    Telefon = a.telefon,
                    Epost = a.epost,
                }).ToList();

                return admin;
            }
        }

        public bool slettAdmin(int id)
        {
            using(var db = new BestillingContext())
            {
                try
                {
                    var slettAdmin = db.admin.FirstOrDefault(a => a.id == id);
                    db.admin.Remove(slettAdmin);
                    db.SaveChanges();
                    return true;
                }
                catch(Exception feil)
                {
                    return false;

                }

            }
        }


        public bool endreAdmin(int id, string fornavn, string etternavn, string telefon, string epost)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    var endreAdmin = db.admin.Find(id);
                    endreAdmin.fornavn = fornavn;
                    endreAdmin.etternavn = etternavn;
                    endreAdmin.telefon = telefon;
                    endreAdmin.epost = epost;
                    db.SaveChanges();
                    return true;
                }
                catch(Exception feil)
                {
                    return false;
                }


            }
        }

        public void leggTilAvganger(int fra, int til)
        {
            var db = new BestillingContext();
            if (fra != til)
            {
                for (int y = 0; y < 23; y += 2)
                {
                    var avgang = new Avgang();
                    avgang.destinasjonFra_id = fra;
                    avgang.destinasjonTil_id = til;
                    int avgangSone = Math.Abs(fra - til);
                    int timer = y;
                    int minutter = random.Next(1, 59);
                    String tid;

                    if (timer < 10 && minutter < 10) tid = "0" + timer + ":0" + minutter;
                    else if (timer < 10 && minutter >= 10) tid = "0" + timer + ":" + minutter;
                    else if (timer >= 10 && minutter < 10) tid = timer + ":0" + minutter;
                    else tid = timer + ":" + minutter;

                    String ankomst = fiksTid(timer, minutter, avgangSone);
                    avgang.tid = tid;
                    avgang.ankomst = ankomst;
                    avgang.time = timer;

                    try
                    {
                        db.avgang.Add(avgang);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error: " + e);
                    }
                }
            }
        }


        private static byte[] lagHash(string innPassord, byte[] innSalt)
        {
            const int keyLength = 24;
            var pbkdf2 = new Rfc2898DeriveBytes(innPassord, innSalt, 1000); // 1000 angir hvor mange ganger hash funskjonen skal utføres for økt sikkerhet
            return pbkdf2.GetBytes(keyLength);
        }

        private static byte[] lagSalt()
        {
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csprng.GetBytes(salt);
            return salt;
        }

//sjekk kobling HomeC->BLL->DAL
        public List<Priser> getPris()
        {
            using(var db = new BestillingContext())
            {
                List<Priser> priser = db.pris.Select(p => new Priser
                {
                    id = p.id,
                    Voksenpris = p.prisVoksen,
                    Studentpris = p.prisStudent,
                    Barnepris = p.prisBarn,
                    Ungdompris = p.prisUngdom,
                    Honnorpris = p.prisHonnor,
                    Vernepliktpris = p.prisVerneplikt,
                    PrisPerSone = p.prisPerSone

                }).ToList();

                return priser;
            }
        }
//sjekk kobling HomeC->BLL->DAL
        public bool endrePris(int id, int prisVoksen, int prisStudent, int prisBarn, int prisUngdom, int prisHonnor, int prisVerneplikt, int prisPerSone)
        {
            using(var db = new BestillingContext())
            {
                try
                {
                    var endrePris = db.pris.Find(id);
                    endrePris.prisVoksen = prisVoksen;
                    endrePris.prisStudent = prisStudent;
                    endrePris.prisBarn = prisBarn;
                    endrePris.prisUngdom = prisUngdom;
                    endrePris.prisHonnor = prisHonnor;
                    endrePris.prisVerneplikt = prisVerneplikt;
                    endrePris.prisPerSone = prisPerSone;
                    db.SaveChanges();
                    return true;

                }
                catch(Exception feil)
                {
                    return false;
                }
            }


        }

        public bool admin_i_db(Admin innAdmin)
        {
            using (var db = new BestillingContext())
            {
                dbAdmin funnetAdmin = db.admin.FirstOrDefault(a => a.epost == innAdmin.Epost);
                if (funnetAdmin != null)
                {
                    byte[] passordForTest = lagHash(innAdmin.Passord, funnetAdmin.salt);
                    bool riktigBruker = funnetAdmin.passord.SequenceEqual(passordForTest);  // merk denne testen!
                    return riktigBruker;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool registrerAdmin(RegistrerAdmin innAdmin)
        {
            using (var db = new BestillingContext())
            {
                dbAdmin fantAdmin = db.admin.FirstOrDefault(a => a.epost == innAdmin.Epost);

                if (fantAdmin == null)
                {
                    var nyAdmin = new dbAdmin();
                    byte[] salt = lagSalt();
                    byte[] hash = lagHash(innAdmin.Passord, salt);
                    nyAdmin.fornavn = innAdmin.Fornavn;
                    nyAdmin.etternavn = innAdmin.Etternavn;
                    nyAdmin.telefon = innAdmin.Telefon;
                    nyAdmin.epost = innAdmin.Epost;
                    nyAdmin.passord = hash;
                    nyAdmin.salt = salt;
                    db.admin.Add(nyAdmin);
                    db.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public string getDestinasjoner()
        {
            var db = new BestillingContext();

            var str = db.destinasjon.Select(s => new { // det var slikt fra før og det funka :(
                id = s.id,
                sted = s.sted
            }).ToList();
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(str);
            System.Diagnostics.Debug.WriteLine(json);

            return json;
        }

        public List<Avganger> endreAvgang(int id, int destFra, int destTil, string avgang, string ankomst)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    var endreAvgang = db.avgang.Find(id);
                    endreAvgang.tid = avgang;
                    endreAvgang.ankomst = ankomst;
                    db.SaveChanges();
                }
                catch (Exception feil)
                {
                    //Exception
                }
                return alleAvganger(destFra, destTil, 00);
            }
        }

        public List<Avganger> slettAvgang(int id, int destFra, int destTil)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    var slettAvgang = db.avgang.Find(id);
                    db.avgang.Remove(slettAvgang);
                    db.SaveChanges();
                }
                catch (Exception feil)
                {
                    //Exception
                }
                return alleAvganger(destFra, destTil, 00);
            }
        }
    }
}
