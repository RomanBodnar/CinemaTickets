using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CinemaTickets.Domain.Abstract;
using CinemaTickets.Domain.Entities;
using CinemaTickets.Web.Models;
using System.Threading.Tasks;
using System.Net;

namespace CinemaTickets.Web.Controllers
{
    public class OrderController : Controller
    {
        private IUnitOfWork db;
        private IOrderProcessor processor;
        public OrderController(IUnitOfWork uow, IOrderProcessor proc)
        {
            db = uow;
            processor = proc;
        }
        public async Task<ActionResult> SeanceOrder(int? seanceId)
        {
            if (seanceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seance seance = await db.Seances.GetByIdAsync(seanceId);
            if (seance == null)
            {
                return HttpNotFound();
            }
            int selectedIndex = 1;
            seance.Movie = await db.Movies.GetByIdAsync(seance.MovieId);
            Hall hall = await db.Halls.GetByIdAsync(seance.HallId);
            hall.Cinema = await db.Cinemas.GetByIdAsync(hall.CinemaId);
            seance.Hall = hall;

            SelectList rows = new SelectList(seance.Hall.Rows.OrderBy(r => r.Number), "Id", "Number", null);
            ViewBag.Rows = rows;
            
            SelectList seats = new SelectList(seance.Seats.Where(s => s.IsBooked == false && s.Row.Number == selectedIndex),
                                                "Id", "Number", null);
            ViewBag.Seats = seats;
            return View(seance);
        }
        public async Task<ActionResult> GetSeatsForRow(int? seanceId, int? rowId)
        {
            
            Seance seance = await db.Seances.GetByIdAsync(seanceId);

            return PartialView(seance.Seats.Where(s => s.IsBooked == false && s.RowId == rowId).ToList<Seat>());
        }
        
        public async Task<ActionResult> AddTicket(Order order, Place place)
        {
            Seat seat = await db.Seats.GetByIdAsync(place.Seat);
            Seance seance = await db.Seances.GetByIdAsync(seat.SeanceId);
            Ticket ticket = new Ticket();
            ticket.Seance = seance;
            ticket.Seance.Movie = await db.Movies.GetByIdAsync(seance.MovieId);
            ticket.Row = place.Row;
            ticket.Seat = place.Seat;
            order.AddTicket(ticket);

            return RedirectToAction("Checkout"); ;
        }
        public ActionResult Checkout()
        {
            return View(new CustomerDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Order order, CustomerDetails shippingDetails)
        {
            if (order.OrderedTickets.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, you ordered nothing!");
            }
            if (ModelState.IsValid)
            {

                foreach (var ticket in order.OrderedTickets)
                {
                    Seat booked_seat = db.Seats.Find(s => s.Id == ticket.Seat);
                    Seance seance_to_order = db.Seances.Find(s => s.Id == booked_seat.SeanceId);

                    Row row = db.Rows.GetById(ticket.Row);
                    booked_seat.IsBooked = true;
                    seance_to_order.SeatsLeft--;
                    db.Seances.Edit(seance_to_order, seance_to_order.Id);
                    db.Seats.Edit(booked_seat, booked_seat.Id);
                    Cinema cinema = db.Cinemas.Find(c => c.Id == seance_to_order.Hall.CinemaId);
                    ticket.Seat = booked_seat.Number;
                    ticket.Row = row.Number;
                    ticket.Hall = seance_to_order.Hall.Name;
                    ticket.Movie = seance_to_order.Movie.Name;
                    ticket.Cinema = cinema.Name;
                }
                shippingDetails.SavePath = String.Format(AppDomain.CurrentDomain.BaseDirectory + "Store/");
                processor.ProcessOrder(order, shippingDetails);
                order.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

    }
}