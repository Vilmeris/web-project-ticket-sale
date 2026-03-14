using System.Data.Entity;

namespace Web_Programming_Project.Models
{
    public class DbPersonal : DbContext
    {
        
        public DbPersonal() : base("name=DbPersonal")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Question> Questions { get; set; }


        public virtual DbSet<EventPrice> EventPrices { get; set; }

        public DbSet<CreditCard> CreditCards { get; set; }
    }
}


