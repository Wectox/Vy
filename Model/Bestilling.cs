using System.ComponentModel.DataAnnotations;

namespace Gruppeoppgave_1.Model
{
    public class Admin
    {
        [Required(ErrorMessage ="Vennligst oppgi epost")]
        public string Epost { get; set; }
        
        [Required(ErrorMessage ="Vennligst oppgi passord")]
        public string Passord { get; set; }
    }


    public class RegistrerAdmin
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Vennligst oppgi fornavn")]
        public string Fornavn { get; set; }
        
        [Required(ErrorMessage = "Vennligst oppgi etternavn")]
        public string Etternavn { get; set; }

        [Required(ErrorMessage = "Vennligst oppgi telefon")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Vennligst oppgi epost")]
        public string Epost { get; set; }

        [Required(ErrorMessage = "Vennligst oppgi passord")]
        public string Passord { get; set; }
    }


    public class Bestilling
    {
        public int avgangId;
        public int id { get; set; }
        public int? enVei { get; set; }
        public int? turRetur { get; set; }
        public string dato { get; set; }
        public string datoRetur { get; set; }
        public int antallReisende { get; set; }
        public int totalPris { get; set; }
    }


    public class Avganger
    {
        public int id { get; set; }
        public string destinasjonFra { get; set; }
        public string destinasjonTil { get; set; }
        public string tid { get; set; }
        public string ankomst { get; set; }
    }


    public class Destinasjoner
    {
        public int id { get; set; }
        public string sted { get; set; }
        public int sone { get; set; }
       
    }


    public class Priser
    {
        public int id { get; set; }

        public int Voksenpris { get; set; }

        public int Studentpris { get; set; }

        public int Barnepris { get; set; }

        public int Ungdompris { get; set; }

        public int Honnorpris { get; set; }

        public int Vernepliktpris { get; set; }

        public int PrisPerSone { get; set; }
    }

    public class EndringsLoggModel
    {
        public int id { get; set; }
        public string tabell { get; set; }
        public string sisteEndret { get; set; }
        public string beskrivelse { get; set; }
    }
}