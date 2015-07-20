using CinemaTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Concrete
{
    public class EFContext : DbContext
    {
        public EFContext() : base("CinemaTicketsDB") 
        {
         //   Database.SetInitializer<EFContext>(new ArticleDbInitializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hall>()
                .HasMany(s => s.Seances)
                .WithRequired(s => s.Hall)
                .HasForeignKey(s => s.HallId)
                .WillCascadeOnDelete(false);

        }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Seance> Seances { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Row> Rows { get; set; }
    }
    public class ArticleDbInitializer : DropCreateDatabaseAlways<EFContext>
    {
        protected override void Seed(EFContext db)
        {
            db.Movies.Add(new Movie
            {
                Name = "Робот Чаппі",
                ReleaseDate  = new DateTime(2015, 03, 12),
                Duration = 139,
                Genre = "бойовик, трилер",
                Director = "Ніл Бломкамп",
                Scenario = "Ніл Бломкамп, Террі Татчелл",
                Actors = "Х'ю Джекман, Сігурні Уівер, Шарлто Коплі, Дев Патель, Міранда Фрігон, Хосе Пабло Кантільо",
                Description = "Події розгортаються в майбутньому. Технології дозволили групі молодих вчених створити робота, який здатний мислити і відчувати. Новому другові вони дали ім'я Чаппі. Своєю поведінкою він нагадує дитину – невміло рухається, вивчає можливості свого «тіла», вчитиься розмовляти і розуміти, що відбувається навколо. Робот-дитина дивиться мультфільми і малює. Безтурботні для Чаппі і наповнені радістю для його творців дні швидко закінчуються. Робота, що має неймовірні можливості і здатність навчатися, викрадають. Чаппі опиняється в руках злочинців, які хочуть використовувати його як ідеального виконавця своїх ідей."

            });
            db.Cinemas.Add(new Cinema
                {
                    Id = 1,
                    Name = "Кінотеатр ім. Коцюбинського",
                    Street = "Соборна, 38",
                    City = "Вінниця"
                });
            db.Halls.Add(new Hall
                {
                    Id = 1,
                    Name = "Червоний",
                    Seats = 206,
                    CinemaId = 1,
                    Cinema = db.Cinemas.FirstOrDefault(c => c.Id == 1)
                });
            base.Seed(db);
        }
    }
}
