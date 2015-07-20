using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaTickets.Domain.Entities;
using CinemaTickets.Domain.Abstract;

namespace CinemaTickets.Web.Models
{
    public class MovieSeances
    {
        public Movie Movie { get; set; }
        public List<CinemaSeances> Cinemas { get; set; }
        public MovieSeances()
        {
            Cinemas = new List<CinemaSeances>();
        }

    }
    public class CinemaSeances
    {
       // public string CinemaName { get; set; }
        public Cinema Cinema { get; set; }
        public List<DateSeances> DateSeances { get; set; }
        public CinemaSeances()
        {
            DateSeances = new List<DateSeances>();
        }
    }
    
    public class DateSeances
    {
        public DateTime Date { get; set; }
        public List<Seance> Seances { get; set; }
        public DateSeances()
        {
            Seances = new List<Seance>();
        }
    }
}
