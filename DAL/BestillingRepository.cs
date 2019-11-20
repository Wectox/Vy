using Gruppeoppgave_1.Model;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Gruppeoppgave_1.DAL
{
    public class BestillingRepository : IBestillingRepository
    {
        Random random = new Random();

        public bool settInnDestinasjon()
        {
            var db = new BestillingContext();
            List<Destinasjon> destinasjonerfra = db.destinasjon.ToList();
            if (destinasjonerfra.Count > 0) //Her sjekker koden om det er en tom destinasjonstabell, hvis den er tom så returnerer den og skriver ut
            {
                Console.Write("Tabellen er ikke tom");
                return true;
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
                return true;
            }
            catch (Exception e)
            {
                /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                Logger logger = LogManager.GetLogger("logging");
                logger.Error(e, "ERROR: Får ikke lagt til destinasjoner i tabellen!");
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

            if (minutter / 60 > 0)
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


        public bool settInnAvganger()
        {
            try
            {
                var db = new BestillingContext();
                List<Avgang> avganger = db.avgang.ToList();
                List<Destinasjon> destinasjoner = db.destinasjon.ToList();


                if (avganger.Count > 0) //Sjekker om avganger er tom
                {
                    return true;
                }

                for (int i = 1; i < destinasjoner.Count + 1; i++)
                {
                    for (int j = 1; j < destinasjoner.Count + 1; j++)
                    {
                        leggTilAvganger(i, j);
                    }
                }
                return true;
            }
            catch(Exception feil)
            {
                /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                Logger logger = LogManager.GetLogger("logging");
                logger.Error(feil, "ERROR: Får ikke satt inn avganger i tabellen");
                throw new Exception("Får ikke lagt inn verdier i tabellen" + feil);
            }
        }


        public List<Avganger> alleAvganger(int destFra, int destTil, int tid)
        {
            var db = new BestillingContext();
            System.Diagnostics.Debug.WriteLine("Tid: " + tid);
            List<Avgang> alleAvganger = db.avgang.Where(a => a.destinasjonFra_id == destFra && a.destinasjonTil_id == destTil && a.time >= tid).ToList();
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
                returnerAvganger.Add(a);
            }

            return returnerAvganger;
        }


        public List<Avganger> alleReturAvganger(int destFra, int destTil, int tid)
        {
            var db = new BestillingContext();
            System.Diagnostics.Debug.WriteLine("TidRetur: " + tid);
            List<Avgang> alleAvganger = db.avgang.Where(a => a.destinasjonFra_id == destTil && a.destinasjonTil_id == destFra && a.time >= tid).ToList();
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
                returnerAvganger.Add(a);
            }

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

        public bool settInnPris()
        {
            var db = new BestillingContext();
            List<Pris> priser = db.pris.ToList();
            if (priser.Count > 0) //Sjekker om pristabellen er tom
            {
                return true;
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
                return true;
            }
            catch (Exception e)
            {
                /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                Logger logger = LogManager.GetLogger("logging");
                logger.Error(e, "ERROR: Får ikke listet opp priser i tabellen");
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
                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Bestillinger";
                    nyLogg.beskrivelse = "Lagt til ny bestilling med id: " + avgangId;
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.bestillinger.Add(nyBestilling);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke lagt inn envei bestillingen!");
                    throw new Exception("Får ikke lagt inn bestillingen: " + feil);
                }
            }
        }

        public bool settInnBestillingTurRetur(int avgangId, int returId, string dato, string datoRetur, int antallReisende, int totalSum)
        {
            using (var db = new BestillingContext())
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
                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Bestillinger";
                    nyLogg.beskrivelse = "Lagt til ny bestilling med id: " + avgangId;
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.bestillinger.Add(nyBestilling);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke lagt inn retur bestillingen!");
                    throw new Exception("Får ikke lagt inn bestillingen: " + feil);
                }
            }
        }


        public List<Destinasjoner> hentDestinasjon()
        {
            using (var db = new BestillingContext())
            {
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

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Destinasjoner";
                    nyLogg.beskrivelse = "Destinasjon med id: " + id + " er endret";
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke endret destinasjonen!");
                    throw new Exception("Får ikke endre destinasjoner: " + feil);
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
                    foreach (Avgang a in avganger)
                    {
                        db.avgang.Remove(a);
                    }

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Destinasjoner";
                    nyLogg.beskrivelse = "Destinasjon med id: " + id + " er slettet";
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    var nyLogg2 = new endringsLogg();
                    nyLogg2.tabell = "Avganger";
                    nyLogg2.beskrivelse = "Alle avganger knyttet til destinasjon_id " + id + " er slettet";
                    nyLogg2.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg2);

                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Det er ikke mulig å slette Destinasjon som er allerede i bestillingen!");
                    return false;
                    //throw new Exception("Får ikke slettet destinasjoner: " + feil);

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
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke listet opp destinasjoner!");
                    throw new Exception("Får ikke lagt inn destinasjonen: " + feil);
                }

                Destinasjon nyDest = db.destinasjon.OrderByDescending(d => d.id).FirstOrDefault(); //LastOrDefault funket ikke

                List<Destinasjoner> destinasjoner = db.destinasjon.Select(dest => new Destinasjoner
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

                try
                {
                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Destinasjoner";
                    nyLogg.beskrivelse = "Lagt til ny destinasjon: " + sted + " sone: " + sone;
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    var nyLogg2 = new endringsLogg();
                    nyLogg2.tabell = "Avganger";
                    nyLogg2.beskrivelse = "Lagt til avganger som binder sammen " + sted + " med alle andre destinasjoner";
                    nyLogg2.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg2);
                    db.SaveChanges();
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "Får ikke listet opp destinasjoner i loggen");
                    throw new Exception("Får ikke listet destinasjoner: " + feil);
                }

                return destinasjoner;
            }
        }

        public List<Bestilling> getBestillinger()
        {
            using (var db = new BestillingContext())
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

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Bestillinger";
                    nyLogg.beskrivelse = "Slettet bestilling med id: " + id;
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke slettet bestillingen!");
                    throw new Exception("Får ikke lagt til verdier til databasen, feil: " + feil);
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

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Bestillinger";
                    nyLogg.beskrivelse = "Endret bestilling med id: " + id;
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke endret bestillinger!");
                    throw new Exception("Får ikke endret verdier i databasen, feil: " + feil);
                }
            }
        }

        public List<RegistrerAdmin> getAdmin()
        {
            using (var db = new BestillingContext())
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
            using (var db = new BestillingContext())
            {
                try
                {
                    var slettAdmin = db.admin.FirstOrDefault(a => a.id == id);
                    db.admin.Remove(slettAdmin);

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Admin";
                    nyLogg.beskrivelse = "Slettet admin med id: " + id;
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke slettet ADMIN!");
                    throw new Exception("Får ikke slettet admin, feil: " + feil);
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

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Admin";
                    nyLogg.beskrivelse = "Endret admin med id: " + id;
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke endret ADMIN!");
                    throw new Exception("Får ikke endret admin, feil: " + feil);
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
                    catch (Exception feil)
                    {
                        /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                        Logger logger = LogManager.GetLogger("logging");
                        logger.Error(feil, "ERROR: Får ikke lagt til Avgangene!");
                        throw new Exception("Error: " + feil);
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
            using (var db = new BestillingContext())
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
            using (var db = new BestillingContext())
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

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Priser";
                    nyLogg.beskrivelse = "Endret priser";
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.SaveChanges();
                    return true;

                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke endret Priser!");
                    throw new Exception("Får ikke endret priser, feil: " + feil);
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

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Admin";
                    nyLogg.beskrivelse = "Registrert ny admin";
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }


        public bool endreAvgang(int id, int destFra, int destTil, string avgang, string ankomst)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    var endreAvgang = db.avgang.Find(id);
                    endreAvgang.tid = avgang;
                    endreAvgang.ankomst = ankomst;

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Avganger";
                    nyLogg.beskrivelse = "Endret avgang med id: " + id;
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke endret Avganger!");
                    throw new Exception("Får ikke endre avganger: " + feil);
                }

            }
        }


        public bool slettAvgang(int id, int destFra, int destTil)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    var slettAvgang = db.avgang.Find(id);
                    db.avgang.Remove(slettAvgang);

                    var nyLogg = new endringsLogg();
                    nyLogg.tabell = "Avganger";
                    nyLogg.beskrivelse = "Slettet avgang med id: " + id;
                    DateTime time = DateTime.Now;
                    nyLogg.sisteEndret = time.ToString();
                    db.logg.Add(nyLogg);

                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke slettet avganger!");
                    throw new Exception("Får ikke slettet avgang, feil: " + feil);
                }

            }
        }

        public List<EndringsLoggModel> hentLogg()
        {
            using (var db = new BestillingContext())
            {
                List<EndringsLoggModel> hentetLogg = db.logg.Select(logg => new EndringsLoggModel
                {
                    id = logg.id,
                    tabell = logg.tabell,
                    sisteEndret = logg.sisteEndret,
                    beskrivelse = logg.beskrivelse

                }).ToList();
                return hentetLogg;
            }
        }

        public bool slettLogg(int id)
        {
            using (var db = new BestillingContext())
            {
                try
                {
                    var logg = db.logg.Find(id);
                    db.logg.Remove(logg);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    /* Denne logg - filen ligger i:  ~\ITPE3200WebApplication\Gruppeoppgave 1\loggingFeil.log  */
                    Logger logger = LogManager.GetLogger("logging");
                    logger.Error(feil, "ERROR: Får ikke slettet logg!");
                    throw new Exception("Får ikke slettet Logg: " + feil);
                }
            }
        }
    }
}
