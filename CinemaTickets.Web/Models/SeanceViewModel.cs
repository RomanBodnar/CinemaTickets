using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CinemaTickets.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.Web.Models
{
    public class SeanceViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [DataType(DataType.Time)]
       // [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Time { get; set; }
        public int Hall_ID { get; set; }
        public int Movie_ID { get; set; }
        public int Price { get; set; }
        public string CinemaName { get; set; }
        
    }
}