using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace BetaCinema.Entities
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Banners> banners { get; set; }
        public virtual DbSet<Bill> bills { get; set; }
        public virtual DbSet<BillFoods> billFoods { get; set; }
        public virtual DbSet<BillStatuses> billStatuses { get; set; }
        public virtual DbSet<BillTickets> billTickets { get; set; }
        public virtual DbSet<Cinema> cinemas { get; set; }
        public virtual DbSet<ConfirmEmails> confirmEmails { get; set; }
        public virtual DbSet<Food> foods { get; set; }
        public virtual DbSet<GeneralSettings> generalSettings { get; set; }
        public virtual DbSet<Movies> movies { get; set; }
        public virtual DbSet<MovieType> movieTypes { get; set; }
        public virtual DbSet<Promotions> promotions { get; set; }
        public virtual DbSet<RankCustomers> rankCustomers { get; set; }
        public virtual DbSet<Rate> rate { get; set; }
        public virtual DbSet<RefreshTokens> refreshTokens { get; set; }
        public virtual DbSet<Role> roles { get; set; }
        public virtual DbSet<Room> rooms { get; set; }
        public virtual DbSet<Schedules> schedules { get; set; }
        public virtual DbSet<Seat> seats { get; set; }
        public virtual DbSet<SeatStatus> seatsStatus { get; set; }
        public virtual DbSet<SeatType> seatTypes { get; set; }
        public virtual DbSet<Tickets> tickets { get; set; }
        public virtual DbSet<Users> users { get; set; }
        public virtual DbSet<UserStatus> userStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"Server = DESKTOP-HJRN3CM\\MSSQLSERVER2022; Database = API_WebBetaCinema; Trusted_Connection = True;" +
                $" TrustServerCertificate=True");
        }
    }
}
