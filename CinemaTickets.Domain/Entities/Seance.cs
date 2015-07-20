using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CinemaTickets.Domain.Entities
{
    public class Seance
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Time { get; set; }
        
        public int SeatsLeft { get; set; }
        public int Price { get; set; }
        
        //Navigation properties
        //For hall
        public int HallId { get; set; }
        public virtual Hall Hall { get; set; }
        
        //For movie
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        
        public virtual ICollection<Seat> Seats { get; set; }
        public Seance()
        {
            Seats = new List<Seat>();
        }

    }
}
