using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.Domain.Entities
{
    public class Hall
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public int Seats { get; set; }
        //Collections for seances and rows
        public virtual ICollection<Seance> Seances { get; set; }
        public virtual ICollection<Row> Rows { get; set; }
        //Navigation property for cinema
        [HiddenInput(DisplayValue = false)]
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public Hall()
        {
            Seances = new List<Seance>();
            Rows = new List<Row>();
        }
    }
}
