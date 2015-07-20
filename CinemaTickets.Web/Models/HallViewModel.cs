using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaTickets.Domain.Abstract;
using CinemaTickets.Domain.Entities;

namespace CinemaTickets.Web.Models
{
    public class HallViewModel
    {
        public string Name { get; set; }
        public int Seats { get; set; }
        
    }
    public class RowViewModel
    {
        public int SeatsNumber { get; set; }
    }
}
