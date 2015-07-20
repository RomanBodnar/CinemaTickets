using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsBooked { get; set; }

        public int RowId { get; set; }
        public virtual Row Row { get; set; }
        public int SeanceId { get; set; }
        public virtual Seance Seance { get; set; }
    }
}
