using System;
using System.Collections.Generic;
using Gruppeoppgave_1.Model;

namespace Gruppeoppgave_1.DAL
{
    public class BestillingRepositoryStub : IBestillingRepository
    {
        
        public bool admin_i_db(Admin innAdmin)
        {
            var nyAdmin = new Admin()
            {
                Epost = "test@ok.com",
                Passord = "ok123"
            };

            if (innAdmin.Epost == nyAdmin.Epost && innAdmin.Passord == nyAdmin.Passord)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Avganger> alleAvganger(int destFra, int destTil, int tid)
        {
            var listAvganger = new List<Avganger>();
            var avgang = new Avganger()
            {
                id = 1,
                destinasjonFra = "Oslo",
                destinasjonTil = "Bergen",
                tid = "10:00",
                ankomst = "11:00"
            };

            listAvganger.Add(avgang);
            listAvganger.Add(avgang);
            listAvganger.Add(avgang);

            return listAvganger;
        }

    

        public List<Avganger> alleReturAvganger(int destFra, int destTil, int tid)
        {
            var listAvgangerRetur = new List<Avganger>();
            var avgangRetur = new Avganger()
            {
                id = 2,
                destinasjonFra = "Drammen",
                destinasjonTil = "Trondheim",
                tid = "12:00",
                ankomst = "20:00"
            };

            listAvgangerRetur.Add(avgangRetur);
            listAvgangerRetur.Add(avgangRetur);
            listAvgangerRetur.Add(avgangRetur);

            return listAvgangerRetur;
        }

        public bool endreAdmin(int id, string fornavn, string etternavn, string telefon, string epost)
        {
            if(id >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool endreBestilling(int id, string dato, string datoRetur, int antallReisende, int totalPris)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool endreDestinasjoner(int id, string sted, int sone)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool endrePris(int id, int prisVoksen, int prisStudent, int prisBarn, int prisUngdom, int prisHonnor, int prisVerneplikt, int prisPerSone)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public List<RegistrerAdmin> getAdmin()
        {
            var listBestilling = new List<RegistrerAdmin>();
            var admin = new RegistrerAdmin()
            {
                id = 1,
                Fornavn = "Ole",
                Etternavn = "Pettersen",
                Telefon = "45652398",
                Epost = "Ole@gmail.com",
                Passord = "OlePetterson"
            };

            listBestilling.Add(admin);
            listBestilling.Add(admin);
            listBestilling.Add(admin);
            return listBestilling;

        }

        public List<Bestilling> getBestillinger()
        {
            var listBestilling = new List<Bestilling>();
            var bestilling = new Bestilling()
            {
                id = 1,
                enVei = 1,
                turRetur = 2,
                dato = "2019-10-24",
                datoRetur = "2019-10-25",
                antallReisende = 1,
                totalPris = 60
            };

            listBestilling.Add(bestilling);

            return listBestilling;
        }
        

        public List<Priser> getPris()
        {
            var listPris = new List<Priser>();
            var pris = new Priser()
            {
                id = 1,
                Voksenpris = 60,
                Barnepris = 10,
                Studentpris = 30,
                Ungdompris = 25,
                Honnorpris = 10,
                Vernepliktpris = 50,
                PrisPerSone = 20
            };

            listPris.Add(pris);
           
            return listPris;
        }

        public List<Destinasjoner> hentDestinasjon()
        {
            var listDestinasjon = new List<Destinasjoner>();
            var destinasjon = new Destinasjoner()
            {
                id = 1,
                sted = "Oslo",
                sone = 1,
            };

            listDestinasjon.Add(destinasjon);

            return listDestinasjon;
        }

        public Pris hentPriser()
        {
            throw new NotImplementedException();
        }


        public Avganger hentValgtAvgang(int destFra, int destTil)
        {
            var avgang = new Avganger()
            {
                id = 1,
                destinasjonFra = "Oslo",
                destinasjonTil = "Bergen",
                tid = "10:00",
                ankomst = "11:00"
            };

            return avgang;
        }


        public List<Destinasjoner> leggInnDestinasjon(string sted, int sone)
        {
            var listDestinasjon = new List<Destinasjoner>();
            var destinasjon = new Destinasjoner()
            {
                id = 12,
                sted = "Asker",
                sone = 2,
            };

            listDestinasjon.Add(destinasjon);

            return listDestinasjon;
        }


        public bool registrerAdmin(RegistrerAdmin innAdmin)
        {
            if (innAdmin.Epost == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool settInnAvganger()
        {
            var listAvganger = new List<Avganger>();
            var innAvgang = new Avganger()
            {
                id = 1,
                destinasjonFra="Oslo",
                destinasjonTil="Bergen",
                tid="12:00",
                ankomst="22:00"
            };

            listAvganger.Add(innAvgang);
            return true;
        }

        public bool settInnBestillingEnvei(int avgangId, string dato, int antallReisende, int totalSum)
        {
            var listBestilling = new List<Bestilling>();
            var bestilling = new Bestilling()
            {
                avgangId = 1,
                dato = "2019-10-24",
                antallReisende = 1,
                totalPris = 60
            };


            listBestilling.Add(bestilling);

            if(avgangId == 0)
            {
                return false;
            } 
            else
            {
                return true;
            }

        }

        public bool settInnBestillingTurRetur(int avgangId, int returId, string dato, string datoRetur, int antallReisende, int totalSum)
        {
            var listBestilling = new List<Bestillinger>();
            var bestilling = new Bestillinger()
            {
                enVei_id = 1,
                turRetur_id = 2,
                dato = "2019-10-24",
                datoRetur = "2019-10-25",
                totalPris = 1000,
                antallReisende = 1,
        };


            listBestilling.Add(bestilling);

            if (avgangId == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public bool settInnDestinasjon()
        {
            var listDestinasjon = new List<Destinasjoner>();
            var innDest = new Destinasjoner()
            {
                id = 1,
                sted = "Moss",
                sone = 5
            };

            listDestinasjon.Add(innDest);
            return true;
        }


        public bool settInnPris()
        {
            var listPris = new List<Priser>();
            var innPris = new Priser()
            {
                id = 1,
                Voksenpris = 90,
                Barnepris = 20,
                Studentpris = 70,
                Ungdompris = 45,
                Honnorpris = 60,
                Vernepliktpris = 85,
                PrisPerSone = 23
            };

            listPris.Add(innPris);
            return true;
        }


        public bool slettAdmin(int id)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool slettBestilling(int id)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool slettDestinasjon(int id)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool endreAvgang(int id, int destFra, int destTil, string avgang, string ankomst)
        {
            if(id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool slettAvgang(int id, int destFra, int destTil)
        {
            if(id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<EndringsLoggModel> hentLogg()
        {
            var listLogg = new List<EndringsLoggModel>();
            var logging = new EndringsLoggModel()
            {
                id = 1,
                tabell = "Priser",
                sisteEndret = "25.10.2019 15:30:00",
                beskrivelse = "Endret priser"
            };

            listLogg.Add(logging);

            return listLogg;
        }

        public bool slettLogg(int id)
        {
           if(id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
       


