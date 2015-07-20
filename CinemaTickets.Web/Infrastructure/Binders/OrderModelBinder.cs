using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CinemaTickets.Domain.Entities;

namespace CinemaTickets.Web.Infrastructure.Binders
{
    public class OrderModelBinder : IModelBinder
    {
        private const string sessionKey = "Order";
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            Order order = null;
            if (controllerContext.HttpContext.Session != null)
            {
                order = (Order)controllerContext.HttpContext.Session[sessionKey];
            }
            if (order == null)
            {
                order = new Order();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = order;
                }
            }
            return order;
        }
    }
}
