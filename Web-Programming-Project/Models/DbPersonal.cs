using System.Data.Entity;

namespace Web_Programming_Project.Models
{
    public class DbPersonal : DbContext
    {
        // Veritabanı bağlantı cümlesi ismi
        public DbPersonal() : base("name=DbPersonal")
        {
        }

        // Tabloları buraya ekliyoruz
        public DbSet<User> Users { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<EventPrice> EventPrices { get; set; }
    }
}


