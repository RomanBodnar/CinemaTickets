using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Entities
{
    public class Row
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int SeatsNumber { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }

        public int HallId { get; set; }
        public virtual Hall Hall { get; set; }
        public Row()
        {
            Seats = new List<Seat>();
        }
    }
}
