using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CinemaTickets.Domain.Abstract;
using CinemaTickets.Domain.Entities;
using CinemaTickets.Web.Models;
using System.Net;
using System.Threading.Tasks;

namespace CinemaTickets.Web.Controllers
{
    public class SeanceController : Controller
    {
        private IUnitOfWork db;
        public SeanceController(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<ActionResult> ViewSeances(int? cinemaId)
        {
            if (cinemaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cinema cinema = await db.Cinemas.FindAsync(c => c.Id == cinemaId);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }
        public async Task<ActionResult> CreateSeance(int? cinemaId)
        {
            if (cinemaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cinema hall = await db.Cinemas.FindAsync(c => c.Id == cinemaId);
            SeanceViewModel svm = new SeanceViewModel();

            if (hall == null)
            {
                return HttpNotFound();
            }

            svm.CinemaName = hall.Name;
            await PopulateSvmDropdownLists(cinemaId);
            return View(svm);
        }
        [HttpPost]
        public async Task<ActionResult> CreateSeance(SeanceViewModel svm)
        {
            if (ModelState.IsValid)
            {
                Hall hall = await db.Halls.FindAsync(h => h.Id == svm.Hall_ID);
                Movie movie = await db.Movies.FindAsync(m => m.Id == svm.Movie_ID);
                if (hall == null || movie == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                for (; svm.StartDate <= svm.EndDate; svm.StartDate = svm.StartDate.AddDays(1))
                {
                    Seance seance = new Seance
                    {
                        Date = svm.StartDate,
                        Time = svm.Time,
                        SeatsLeft = hall.Seats,
                        Price = svm.Price,
                        MovieId = svm.Movie_ID,
                        HallId = svm.Hall_ID
                    };
                    seance.Movie = movie;
                    seance.Hall = hall;
                    await db.Seances.InsertAsync(seance);
                }
                //Create seats to order for seances
                await CreateSeats();

                TempData["message"] = string.Format("Seances have been saved");
                return RedirectToAction("ViewSeances", new { cinemaId = hall.CinemaId });
            }
            else
            {
                TempData["message"] = string.Format("Seance hasn't been saved");
                return View();
            }
        }
        private async Task CreateSeats()
        {
            ICollection<Seance> new_seances = await db.Seances.FindAllAsync(seance => seance.Seats.Count == 0);
            foreach(Seance seance in new_seances)
            {
                foreach (Row row in seance.Hall.Rows)
                {
                    for (int i = 0; i < row.SeatsNumber; i++)
                    {
                        Seat seat = new Seat
                        {
                            IsBooked = false,
                            Number = i + 1,
                            SeanceId = seance.Id,
                            Seance = seance,
                            RowId = row.Id,
                            Row = row
                        };
                        await db.Seats.InsertAsync(seat);
                    }
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult> DeleteSeance(int seanceId)
        {
            Seance deletedSeance = await db.Seances.GetByIdAsync(seanceId);
            if (deletedSeance != null)
            {
                await db.Seances.DeleteAsync(deletedSeance);
                TempData["message"] = string.Format("{0} was deleted", deletedSeance.Date);
            }
            //Redirect to path&query. SportsStore project
            return RedirectToAction("CinemasList", "Cinema");
        }
        private async Task PopulateSvmDropdownLists(int? cinemaId)
        {
            var hallList = (await db.Halls.FindAllAsync(h => h.CinemaId == cinemaId)).OrderBy(h => h.Seats);
            var movieList = (await db.Movies.GetAllAsync()).OrderBy(m => m.Name);
            
            ViewBag.Movies = new SelectList(movieList, "Id", "Name", null);
            ViewBag.Halls = new SelectList(hallList, "Id", "Name", null);

        }
        
    }
}