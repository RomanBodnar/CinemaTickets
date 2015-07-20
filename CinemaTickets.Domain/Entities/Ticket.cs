using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Entities
{
    public class Order
    {
        private List<Ticket> Tickets = new List<Ticket>(); 
        public void AddTickets(IEnumerable<Ticket> tickets)
        {
            Tickets = tickets as List<Ticket>;
        }
        public void AddTicket(Ticket ticket)
        {
            Tickets.Add(ticket);
        }
        public int ComputeTotalValue()
        {
            return Tickets.Sum(t => t.Seance.Price);
        }
        public void Clear()
        {
            Tickets.Clear();
        }
        public IEnumerable<Ticket> OrderedTickets
        {
            get { return Tickets; }
        }
    }
    
    public class Ticket
    {
        public string Movie { get; set; }
        public string Cinema { get; set; }
        public string Hall { get; set; }
        public int Row { get; set; }
        public int Seat { get; set; }
        public Seance Seance { get; set; }
    }
}
