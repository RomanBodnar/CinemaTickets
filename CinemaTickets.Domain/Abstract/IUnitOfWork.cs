using CinemaTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<Cinema> Cinemas
        {
            get;
        }
        IRepository<Hall> Halls
        {
            get;
        }
        IRepository<Movie> Movies
        {
            get;
        }
        IRepository<Row> Rows
        {
            get;
        }
        IRepository<Seance> Seances
        {
            get;
        }
        IRepository<Seat> Seats
        {
            get;
        }
    }
}
