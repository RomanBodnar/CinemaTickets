using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CinemaTickets.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Scenario { get; set; }
        public string Actors { get; set; }
        public string Description { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        //Collection for table binding(Seance)
        public virtual ICollection<Seance> Seances { get; set; }
        public Movie()
        {
            Seances = new List<Seance>();
        }
    }
}
