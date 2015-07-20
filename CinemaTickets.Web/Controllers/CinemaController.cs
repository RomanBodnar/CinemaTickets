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
using System.Data.Entity.Infrastructure;

namespace CinemaTickets.Web.Controllers
{
    [Authorize]
    public class CinemaController : Controller
    {
        private IUnitOfWork db;

        public CinemaController(IUnitOfWork unit_of_work)
        {
            db = unit_of_work;
        }
        #region Cinema
        [AllowAnonymous]
        public async Task<ActionResult> Cinemas()
        {
            List<Cinema> cinemas = (await db.Cinemas.GetAllAsync()) as List<Cinema>;
            return View(cinemas);
        }
        [AllowAnonymous]
        public async Task<ActionResult> CinemaDetails(int? cinemaId)
        {
            if (cinemaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = await db.Cinemas.FindAsync(art => art.Id == cinemaId);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            CinemaSeances cs = new CinemaSeances();
            IEnumerable<Seance> seances_in_cinema = await db.Seances.FindAllAsync(seance => seance.Hall.CinemaId == cinema.Id );
                
                
               IEnumerable<DateTime> seances_dates = seances_in_cinema.Select(s => s.Date).Distinct().OrderBy(d => d.Date);
                
                
                foreach (var date in seances_dates)
                {
                    DateSeances ds = new DateSeances();
                    ds.Date = date;
                    ds.Seances = seances_in_cinema.Where(s => s.Date == date).OrderBy(s => s.MovieId).ToList<Seance>(); //from seance in movie_seances
                                 //where s
                    cs.DateSeances.Add(ds);
                }
            cs.Cinema = cinema;
            return View(cs);
        }
        // Returns list of cinemas for Admin controller. The action and the view only for administration purpose
        
        public async Task<ActionResult> CinemasList()
        {
            List<Cinema> cinemas = (await db.Cinemas.GetAllAsync()) as List<Cinema>;
            return View(cinemas);
        }
        public ViewResult CreateCinema()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateCinema([Bind(Include = "Name, City, Street")] Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                await db.Cinemas.InsertAsync(cinema);
                TempData["message"] = string.Format("{0} has been saved", cinema.Name);
                return RedirectToAction("CinemasList");
            }
            else
            {
                TempData["message"] = string.Format("Cinema hasn't been saved");
                return View(cinema);
            }
        }
        public async Task<ActionResult> EditCinema(int? cinemaId)
        {
            if (cinemaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = await db.Cinemas.FindAsync(art => art.Id == cinemaId);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }
        [HttpPost]
        public async Task<ActionResult> EditCinema([Bind(Include = "Id, Name, City, Street")]Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                await db.Cinemas.EditAsync(cinema, cinema.Id);
                TempData["message"] = string.Format("{0} has been saved", cinema.Name);
                return RedirectToAction("CinemasList");
            }
            else
            {
                TempData["message"] = string.Format("Cinema hasn't been saved");
                return View(cinema);
            }
        }
        [HttpPost]
        public async Task<ActionResult> DeleteCinema(int cinemaId)
        {
            Cinema deletedCinema = await db.Cinemas.GetByIdAsync(cinemaId);
            if (deletedCinema != null)
            {
                await db.Cinemas.DeleteAsync(deletedCinema);
                TempData["message"] = string.Format("{0} was deleted", deletedCinema.Name);
            }
            return RedirectToAction("CinemasList");
        }

        #endregion
        //TODO Add necessary CRUD functionality. Add corresponding ViewModel
        #region Hall
        public async Task<ActionResult> ViewHalls(int? cinemaId)
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
        public async Task<ActionResult> CreateHall(int? cinemaId)
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
            ViewBag.CinemaId = cinemaId;
            Hall hall = new Hall();
            hall.Cinema = cinema;
            hall.CinemaId = cinema.Id;
            return View(hall);
        }
        [HttpPost]
        public async Task<ActionResult> CreateHall(Hall hall)
        {
            if (ModelState.IsValid)
            {
                Cinema cinema = await db.Cinemas.FindAsync(c => c.Id == hall.CinemaId);

                hall.Cinema = cinema;

                await db.Halls.InsertAsync(hall);
                TempData["message"] = string.Format("Hall {0} has been saved", hall.Name);
                return RedirectToAction("ViewHalls", new { cinemaId = hall.CinemaId });
            }
            else
            {
                TempData["message"] = string.Format("Hall hasn't been saved");
                return View(hall);
            }
        }
        public async Task<ActionResult> EditHall(int? hallId)
        {
            if (hallId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hall hall = await db.Halls.FindAsync(h => h.Id == hallId);
            if (hall == null)
            {
                return HttpNotFound();
            }

            return View(hall);
        }
        [HttpPost]
        public async Task<ActionResult> EditHall(Hall hall)
        {
            if (ModelState.IsValid)
            {
                await db.Halls.EditAsync(hall, hall.Id);
                TempData["message"] = string.Format("Hall {0} has been saved", hall.Name);
                return RedirectToAction("ViewHalls", new { cinemaId = hall.CinemaId });
            }
            else
            {
                TempData["message"] = string.Format("Cinema hasn't been saved");
                return View(hall);
            }
        }
        [HttpPost]
        public async Task<ActionResult> DeleteHall(int hallId)
        {
            Hall deletedHall = await db.Halls.GetByIdAsync(hallId);
            if (deletedHall != null)
            {
                await db.Halls.DeleteAsync(deletedHall);
                TempData["message"] = string.Format("{0} was deleted", deletedHall.Name);
            }
            return RedirectToAction("ViewHalls", new { cinemaId = deletedHall.CinemaId });
        }
        #endregion
        #region Rows
        public async Task<ActionResult> ViewRows(int? hallId)
        {
            if (hallId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Hall hall = await db.Halls.FindAsync(c => c.Id == hallId);
            if (hall == null)
            {
                return HttpNotFound();
            }
            return View(hall);
        }
        [HttpGet]
        public async Task<ActionResult> CreateRow(int? hallId)
        {
            if (hallId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hall hall = await db.Halls.FindAsync(h => h.Id == hallId);
            if (hall == null)
            {
                return HttpNotFound();
            }

            Row row = new Row();
            row.Hall = hall;
            row.HallId = hall.Id;

            return View(row);
        }
        //TODO Add error handling and possibility to add multiply rows simultaneously
        [HttpPost]
        public async Task<ActionResult> CreateRow(IEnumerable<Row> rows)
        {
            if (ModelState.IsValid)
            {
                Row Row = rows.First(); 
                Hall hall = await db.Halls.FindAsync(h => h.Id == Row.HallId);
                foreach (Row row in rows)
                {
                    row.Hall = hall;
                    await db.Rows.InsertAsync(row);
                }
                TempData["message"] = string.Format("Rows have been saved");
                return RedirectToAction("ViewRows", new { hallId = Row.HallId });
            }
            else
            {
                TempData["message"] = string.Format("Rows haven't been saved");
                return View("CinemaList");
            }
        }
        public async Task<ActionResult> DeleteRow(int? rowId)
        {
            Row deletedRow = await db.Rows.GetByIdAsync(rowId);
            if (deletedRow != null)
            {
                await db.Rows.DeleteAsync(deletedRow);
                TempData["message"] = string.Format("{0} was deleted", deletedRow.Number);
            }
            return RedirectToAction("ViewRows", new { hallId = deletedRow.HallId});
        }
        #endregion
    }
}