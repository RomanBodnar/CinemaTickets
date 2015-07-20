using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CinemaTickets.Domain.Entities
{
    
    public class Cinema
    {
        [HiddenInput(DisplayValue=false)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        //Collection for halls
        public virtual ICollection<Hall> Halls { get; set; }
        public Cinema()
        {
            Halls = new List<Hall>();
        }
    }
}
