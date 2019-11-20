using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gruppeoppgave_1.DAL
{

    public class endringsLogg
    {
        public int id { get; set; }
        public string tabell { get; set; }
        public string sisteEndret { get; set; }
        public string beskrivelse { get; set; }
    }


    public class dbAdmin 
    {
        public int id { get; set; }
        public string epost { get; set; }
        public string fornavn { get; set; }
        public string etternavn { get; set; }
        public string telefon { get; set; }
        public byte[] passord { get; set; }
        public byte[] salt { get; set; }
    }


  
    public class Destinasjon
    {
        public int id { get; set; }

        public string sted { get; set; }

        public int sone { get; set; }

        public virtual List<Avgang> avgangFra { get; set; }
        public virtual List<Avgang> avgangTil { get; set; }
    }



    public class Pris 
    {
        public int id { get; set; }

        public int prisVoksen { get; set; }

        public int prisStudent { get; set; }

        public int prisBarn { get; set; }

        public int prisUngdom { get; set; }

        public int prisHonnor { get; set; }

        public int prisVerneplikt { get; set; }
        
        public int prisPerSone { get; set; }
    }


    public class Avgang
    {
        [Key]
        public int id { get; set; }

        public int? destinasjonFra_id { get; set; }

        public int? destinasjonTil_id { get; set; }

        public string tid { get; set; }

        public string ankomst { get; set; }
        
        public int time { get; set; }

        [ForeignKey("destinasjonFra_id")]
        [InverseProperty("avgangFra")]
        public virtual Destinasjon destinasjonFra { get; set; }

        [ForeignKey("destinasjonTil_id")]
        [InverseProperty("avgangTil")]
        public virtual Destinasjon destinasjonTil { get; set; }

        public virtual List<Bestillinger> bestillEnVei { get; set; }

        public virtual List<Bestillinger> bestillTurRetur { get; set; }
    }


    public class Bestillinger
    {
        public int id { get; set; }
        
        public int? enVei_id { get; set; }
        
        public int? turRetur_id { get; set; }

        public string dato { get; set; }

        public string datoRetur { get; set; }

        public int antallReisende { get; set; }

        public int totalPris { get; set; }

        [ForeignKey("enVei_id")]
        [InverseProperty("bestillEnVei")]
        public virtual Avgang avgangEnVei { get; set; }

        [ForeignKey("turRetur_id")]
        [InverseProperty("bestillTurRetur")]
        public virtual Avgang avgangturRetur { get; set; }
    }


    public class BestillingContext : DbContext
    {
        public BestillingContext() : base("Bestilling")
        {
            Database.CreateIfNotExists();
        }

        public DbSet<endringsLogg> logg { get; set; }
        public DbSet<dbAdmin> admin { get; set; }
        public DbSet<Destinasjon> destinasjon { get; set; }

        public DbSet<Pris> pris { get; set; }

        public DbSet<Avgang> avgang{ get; set; }

        public DbSet<Bestillinger> bestillinger { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
