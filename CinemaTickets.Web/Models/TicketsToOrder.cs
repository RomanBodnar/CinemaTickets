using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaTickets.Domain.Entities;

namespace CinemaTickets.Web.Models
{
    public class TicketsToOrder
    {
        public Seance Seance { get; set; }
        public List<Place> Places { get; set; }
        public TicketsToOrder()
        {
            Places = new List<Place>();
        }
    }
    public class Place
    {
        public int Row { get; set; }
        public int Seat { get; set; }
    }
}
