using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Entities
{
    public class CustomerDetails
    {

        [Required(ErrorMessage="Обов’язкове поле має бути заповнене")]
        public string Email { get; set; }
        [ScaffoldColumn(false)]
        public string SavePath {get; set; }
    }
}
