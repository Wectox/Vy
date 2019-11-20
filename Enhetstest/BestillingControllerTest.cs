using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Gruppeoppgave_1.Controllers;
using Gruppeoppgave_1.BLL;
using Gruppeoppgave_1.DAL;
using Gruppeoppgave_1.Model;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using System.Linq;

namespace Enhetstest
{
    [TestClass]
    public class BestillingControllerTest
    {

        [TestMethod]
        public void admin_i_db_OK()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innAdmin = new Admin()
            {
                Epost = "test@ok.com",
                Passord = "ok123"
            };

            //Act
            var result = (bool)controller.admin_i_db(innAdmin);

            //Assert
            Assert.AreEqual(true, result);
        }


        [TestMethod]
        public void admin_i_db_FEIL()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innAdmin = new Admin()
            {
                Epost = "",
                Passord = ""
            };

            //Act
            var result = (bool)controller.admin_i_db(innAdmin);

            //Assert
            Assert.AreEqual(false, result);
        }


        [TestMethod]
        public void list_Admin()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<RegistrerAdmin>();
            var admin = new RegistrerAdmin()
            {
                id = 1,
                Fornavn = "Ole",
                Etternavn = "Pettersen",
                Telefon = "45652398",
                Epost = "Ole@gmail.com",
                Passord = "OlePetterson"
            };

            forventetResultat.Add(admin);
            forventetResultat.Add(admin);
            forventetResultat.Add(admin);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = controller.getAdmin();

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(json, jsonResult);
        }


        [TestMethod]
        public void endreAdmin_funnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var admin = new RegistrerAdmin()
            {
                id = 1,
                Fornavn = "Caroline",
                Etternavn = "Karlsen",
                Telefon = "47862167",
                Epost = "true@Oslo.no",
            };

            //Act
            var jsonResult = (string)controller.endreAdmin(admin.id, admin.Fornavn, admin.Etternavn, admin.Telefon, admin.Epost);

            //Assert
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void endreAdmin_IkkeFunnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var admin = new RegistrerAdmin()
            {
                id = 0,
                Fornavn = "Camilla",
                Etternavn = "Kristoffersen",
                Telefon = "47862167",
                Epost = "false@Oslo.no",
            };

            //Act
            var jsonResult = (string)controller.endreAdmin(admin.id, admin.Fornavn, admin.Etternavn, admin.Telefon, admin.Epost);

            //Assert
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void registrerAdmin()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));

            // Act
            var actionResult = (ViewResult)controller.loggInn();

            // Assert
            Assert.AreEqual(actionResult.ViewName, "");
        }


        [TestMethod]
        public void registrerAdmin_Post_OK()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innAdmin = new RegistrerAdmin();
            innAdmin.Epost = "therese@hotmail.com";

            //Act
            var result = (RedirectToRouteResult)controller.registrerAdmin(innAdmin);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "loggInn");
        }


        [TestMethod]
        public void registrerAdmin_Post_Model_FEIL()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            controller.ViewData.ModelState.AddModelError("Epost", "Vennligst oppgi epost");
            var innAdmin = new RegistrerAdmin();

            //Act
            var result = (ViewResult)controller.registrerAdmin(innAdmin);

            // Assert
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
            Assert.AreEqual(result.ViewName, "");

        }


        [TestMethod]
        public void registrerAdmin_Post_DB_FEIL()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innAdmin = new RegistrerAdmin();
            innAdmin.Epost = "";

            //Act
            var result = (ViewResult)controller.registrerAdmin(innAdmin);

            // Assert
            Assert.AreEqual(result.ViewName, "");
        }


        [TestMethod]
        public void slettAdmin_Funnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innAdmin = new dbAdmin()
            {
                id = 1,
                epost = "piotr.kusnierz@interia.eu",
                fornavn = "Piotr",
                etternavn = "Kusnierz",
                telefon = "12341234",
            };

            // Act
            var jsonResult = (string)controller.slettAdmin(innAdmin.id);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void slettAdmin_IkkeFunnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innAdmin = new dbAdmin()
            {
                id = 0,
                epost = "piotr.kusnierz@interia.eu",
                fornavn = "Piotr",
                etternavn = "Kusnierz",
                telefon = "12341234",
            };

            // Act
            var jsonResult = (string)controller.slettAdmin(innAdmin.id);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void listPris()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Priser>();
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

            forventetResultat.Add(pris);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = (string)controller.getPris();

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(json, jsonResult);
        }


        [TestMethod]
        public void Endre_Pris_funnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
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

            // Act
            var jsonResult = (string)controller.endrePriser(pris.id, pris.Voksenpris, pris.Barnepris, pris.Studentpris, pris.Ungdompris, pris.Honnorpris, pris.Vernepliktpris, pris.PrisPerSone);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);

        }


        [TestMethod]
        public void Endre_Pris_Ikke_funnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var pris = new Priser()
            {
                id = 0,
                Voksenpris = 60,
                Barnepris = 10,
                Studentpris = 30,
                Ungdompris = 25,
                Honnorpris = 10,
                Vernepliktpris = 50,
                PrisPerSone = 20
            };
            controller.ViewData.ModelState.AddModelError("", "");

            // Act
            var jsonResult = (string)controller.endrePriser(pris.id, pris.Voksenpris, pris.Barnepris, pris.Studentpris, pris.Ungdompris, pris.Honnorpris, pris.Vernepliktpris, pris.PrisPerSone);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void hentDestinasjon()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Destinasjoner>();
            var destinasjon = new Destinasjoner()
            {
                id = 1,
                sted = "Oslo",
                sone = 1
            };

            forventetResultat.Add(destinasjon);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = (string)controller.hentDestinasjon();

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(json, jsonResult);
        }


        [TestMethod]
        public void leggInnDestinasjon()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Destinasjoner>();
            var destinasjon = new Destinasjoner()
            {
                id = 12,
                sted = "Asker",
                sone = 2
            };

            forventetResultat.Add(destinasjon);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = (string)controller.nyDestinasjon(destinasjon.sted, destinasjon.sone);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(json, jsonResult);
        }


        [TestMethod]
        public void slettDestinasjon_Funnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innDestinasjon = new Destinasjon()
            {
                id = 1,
                sted = "Oslo",
                sone = 1
            };

            // Act
            var jsonResult = (string)controller.slettDestinasjon(innDestinasjon.id);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void slettDestinasjon_IkkeFunnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innDestinasjon = new Destinasjon()
            {
                id = 0,
                sted = "Oslo",
                sone = 1
            };

            // Act
            var jsonResult = (string)controller.slettDestinasjon(innDestinasjon.id);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void endreDestinasjon_Funnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innDestinasjon = new Destinasjon()
            {
                id = 1,
                sted = "Oslo",
                sone = 1
            };

            // Act
            var jsonResult = (string)controller.endreDestinasjoner(innDestinasjon.id, innDestinasjon.sted, innDestinasjon.sone);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void endreDestinasjon_IkkeFunnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var innDestinasjon = new Destinasjon()
            {
                id = 0,
                sted = "Oslo",
                sone = 1
            };

            // Act
            var jsonResult = (string)controller.endreDestinasjoner(innDestinasjon.id, innDestinasjon.sted, innDestinasjon.sone);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void getBestillinger()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Bestilling>();
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

            forventetResultat.Add(bestilling);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = (string)controller.getBestillinger();

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(json, jsonResult);
        }


        [TestMethod]
        public void list_Avganger()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Avganger>();
            var avgang = new Avganger()
            {
                id = 1,
                destinasjonFra = "Oslo",
                destinasjonTil = "Bergen",
                tid = "10:00",
                ankomst = "11:00"
            };

            forventetResultat.Add(avgang);
            forventetResultat.Add(avgang);
            forventetResultat.Add(avgang);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = controller.hentAvganger(avgang.id, 1, 2);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(json, jsonResult);
        }


        [TestMethod]
        public void list_Avganger_Retur()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Avganger>();
            var avgangRetur = new Avganger()
            {
                id = 2,
                destinasjonFra = "Drammen",
                destinasjonTil = "Trondheim",
                tid = "12:00",
                ankomst = "20:00"
            };

            forventetResultat.Add(avgangRetur);
            forventetResultat.Add(avgangRetur);
            forventetResultat.Add(avgangRetur);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = controller.hentReturAvganger(avgangRetur.id, 1, 2);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(json, jsonResult);

        }

        
        [TestMethod]
        public void hentValgtAvgang()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));

            var valgtAvgang = new Avganger()
            {
                id = 1,
                destinasjonFra = "Oslo",
                destinasjonTil = "Bergen",
                tid = "10:00",
                ankomst = "11:00"
            };

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(valgtAvgang);

            //Act

            var jsonResult = controller.hentAvgangInfo(valgtAvgang.id, 1);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(json, jsonResult);
        }
        

        [TestMethod]
        public void slettBestilling_Funnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Bestilling>();
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

            // Act
            var jsonResult = (string)controller.slettBestilling(bestilling.id);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void slettBestilling_IkkeFunnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Bestilling>();
            var bestilling = new Bestilling()
            {
                id = 0,
                enVei = 1,
                turRetur = 2,
                dato = "2019-10-24",
                datoRetur = "2019-10-25",
                antallReisende = 1,
                totalPris = 60
            };

            // Act
            var jsonResult = (string)controller.slettBestilling(bestilling.id);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void endreBestilling_Funnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var bestillling = new Bestilling()
            {
                id = 1,
                dato = "2019-10-24",
                datoRetur = "2019-10-25",
                antallReisende = 1,
                totalPris = 60
            };

            // Act
            var jsonResult = (string)controller.endreBestilling(bestillling.id, bestillling.dato, bestillling.datoRetur, bestillling.antallReisende, bestillling.totalPris);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void endreBestilling_IkkeFunnet()
        {
            // Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var bestillling = new Bestilling()
            {
                id = 0,
                dato = "2019-10-24",
                datoRetur = "2019-10-25",
                antallReisende = 1,
                totalPris = 60
            };

            // Act
            var jsonResult = (string)controller.endreBestilling(bestillling.id, bestillling.dato, bestillling.datoRetur, bestillling.antallReisende, bestillling.totalPris);

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void settInnBestillingEnvei_OK()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Bestilling>();
            var bestilling = new Bestilling()
            {
                id = 1,
                dato = "2019-10-24",
                antallReisende = 1,
                totalPris = 60
            };

            forventetResultat.Add(bestilling);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = (string)controller.leggBestillingEnvei(bestilling.id, bestilling.dato, bestilling.antallReisende, bestilling.totalPris);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void settInnBestillingEnvei_Feil()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Bestilling>();
            var bestilling = new Bestilling()
            {
                id = 0,
                dato = "2019-10-24",
                antallReisende = 1,
                totalPris = 60
            };

            forventetResultat.Add(bestilling);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = (string)controller.leggBestillingEnvei(bestilling.id, bestilling.dato, bestilling.antallReisende, bestilling.totalPris);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void settInnBestillingTurRetur_OK()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Bestillinger>();
            var bestilling = new Bestillinger()
            {
                enVei_id = 1,
                turRetur_id = 2,
                dato = "2019-10-24",
                datoRetur = "2019-10-25",
                totalPris = 1000,
                antallReisende = 1,
            };

            forventetResultat.Add(bestilling);

            //Act
            int enVeiID = (int)(bestilling.enVei_id);
            int turReturID = (int)(bestilling.enVei_id);
            var jsonResult = (string)controller.leggBestillingTurRetur(enVeiID, turReturID, bestilling.dato, bestilling.datoRetur, bestilling.totalPris, bestilling.antallReisende);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void settInnBestillingTurRetur_Feil()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Bestillinger>();
            var bestilling = new Bestillinger()
            {
                enVei_id = 0,
                turRetur_id = 2,
                dato = "2019-10-24",
                datoRetur = "2019-10-25",
                totalPris = 1000,
                antallReisende = 1,
            };

            forventetResultat.Add(bestilling);
            forventetResultat.Add(bestilling);

            //Act
            int enVeiID = (int)(bestilling.enVei_id);
            int turReturID = (int)(bestilling.enVei_id);
            var jsonResult = (string)controller.leggBestillingTurRetur(enVeiID, turReturID, bestilling.dato, bestilling.datoRetur, bestilling.totalPris, bestilling.antallReisende);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void endreAvganger_Funnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Avgang>();
            var avgang = new Avgang()
            {
                id = 1,
                destinasjonFra_id = 1,
                destinasjonTil_id = 2,
                tid = "10:00",
                ankomst = "17:00"
            };

            forventetResultat.Add(avgang);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            int destFra = (int)(avgang.destinasjonFra_id);
            int destTil = (int)(avgang.destinasjonTil_id);
            var jsonResult = (string)controller.endreAvgang(avgang.id, destFra, destTil, avgang.tid, avgang.ankomst);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void endreAvganger_IkkeFunnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<Avgang>();
            var avgang = new Avgang()
            {
                id = 0,
                destinasjonFra_id = 5,
                destinasjonTil_id = 8,
                tid = "23:00",
                ankomst = "00:00"
            };

            forventetResultat.Add(avgang);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            int destFra = (int)(avgang.destinasjonFra_id);
            int destTil = (int)(avgang.destinasjonTil_id);
            var jsonResult = (string)controller.endreAvgang(avgang.id, destFra, destTil, avgang.tid, avgang.ankomst);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void slettAvgang_Funnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var hentAvgang = new Avgang()
            {
                id = 1,
                destinasjonFra_id = 2,
                destinasjonTil_id = 3,
                tid = "10:00",
                ankomst = "18:00"
            };

            //Act
            int destFra = (int)(hentAvgang.destinasjonFra_id);
            int destTil = (int)(hentAvgang.destinasjonTil_id);
            var jsonResult = controller.slettAvgang(hentAvgang.id, destFra, destTil);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }

        [TestMethod]
        public void slettAvgang_IkkeFunnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var hentAvgang = new Avgang()
            {
                id = 0,
                destinasjonFra_id = 2,
                destinasjonTil_id = 3,
                tid = "10:00",
                ankomst = "18:00"
            };

            //Act
            int destFra = (int)(hentAvgang.destinasjonFra_id);
            int destTil = (int)(hentAvgang.destinasjonTil_id);
            var jsonResult = controller.slettAvgang(hentAvgang.id, destFra, destTil);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void slettLogg_Funnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var hentLogg = new EndringsLoggModel();
            hentLogg.id = 1;

            //Act
            var jsonResult = controller.slettLogg(hentLogg.id);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("true", jsonResult);
        }


        [TestMethod]
        public void slettLogg_IkkeFunnet()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var hentLogg = new EndringsLoggModel();
            hentLogg.id = 0;

            //Act
            var jsonResult = controller.slettLogg(hentLogg.id);

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("false", jsonResult);
        }


        [TestMethod]
        public void hentLogg()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));
            var forventetResultat = new List<EndringsLoggModel>();
            var logg = new EndringsLoggModel()
            {
                id = 1,
                tabell = "Priser",
                sisteEndret = "25.10.2019 15:30:00",
                beskrivelse = "Endret priser"
            };

            forventetResultat.Add(logg);

            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(forventetResultat);

            //Act
            var jsonResult = (string)controller.hentLogg();

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(json, jsonResult);
        }


        [TestMethod]
        public void settInnDestinasjoner()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));

            //Act
            var resultat = (bool)controller.settInnDestinasjon();

            //Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual(true, resultat);
        }


        [TestMethod]
        public void settInnAvganger()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));

            //Act
            var resultat = (bool)controller.settInnAvganger();

            //Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual(true, resultat);
        }


        [TestMethod]
        public void settInnPris()
        {
            //Arrange
            var controller = new HomeController(new BestillingLogikk(new BestillingRepositoryStub()));

            //Act
            var resultat = (bool)controller.settInnPris();

            //Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual(true, resultat);
        }
    }
}
