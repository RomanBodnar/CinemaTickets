using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaTickets.Domain.Abstract;
using CinemaTickets.Domain.Entities;

namespace CinemaTickets.Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private EFContext dbContext = new EFContext();
        private IRepository<Cinema> cinema_repository;
        private IRepository<Hall> hall_repository;
        private IRepository<Movie> movie_repository;
        private IRepository<Row> row_repository;
        private IRepository<Seance> seance_repository;
        private IRepository<Seat> seat_repository;

        public UnitOfWork()
        {
            cinema_repository = new GenericRepository<Cinema>(dbContext);
            hall_repository = new GenericRepository<Hall>(dbContext);
            movie_repository = new GenericRepository<Movie>(dbContext);
            row_repository = new GenericRepository<Row>(dbContext);
            seance_repository = new GenericRepository<Seance>(dbContext);
            seat_repository = new GenericRepository<Seat>(dbContext);
        }

        public IRepository<Cinema> Cinemas
        {
            get
            {
                return cinema_repository;
            }
        }
        public IRepository<Hall> Halls
        {
            get
            {
                return hall_repository;
            }
        }
        public IRepository<Movie> Movies
        {
            get
            {
                return movie_repository;
            }
        }
        public IRepository<Row> Rows
        {
            get
            {
                return row_repository;
            }
        }
        public IRepository<Seance> Seances
        {
            get
            {
                return seance_repository;
            }
        }
        public IRepository<Seat> Seats
        {
            get
            {
                return seat_repository;
            }
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
