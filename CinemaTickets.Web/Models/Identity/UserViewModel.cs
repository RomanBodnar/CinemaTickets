using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Web.Models.Identity
{
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
