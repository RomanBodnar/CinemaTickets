using CinemaTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Abstract
{
    public interface IOrderProcessor
    {
        void ProcessOrder(Order order, CustomerDetails details);
    }
}
