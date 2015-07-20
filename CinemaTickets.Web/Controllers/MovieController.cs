using CinemaTickets.Domain.Abstract;
using CinemaTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CinemaTickets.Web.Models;

namespace CinemaTickets.Web.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private IUnitOfWork db;
        public MovieController(IUnitOfWork unit_of_work)
        {
            db = unit_of_work;
        }

        // Returns list of movies for Home/Movies. Redirect
        [AllowAnonymous]
        public async Task<ActionResult> Movies()
        {
            List<Movie> movies = (await db.Movies.FindAllAsync(m => m.ReleaseDate <= DateTime.Now)) as List<Movie>; ;
            return View(movies);
        }
        // Returns list of movies that will be released soon for Home/Soon. Redirect. Same view as for Movies   
        [AllowAnonymous]
        public async Task<ActionResult> Soon()
        {
            List<Movie> soon_movies = (await db.Movies.FindAllAsync(m => m.ReleaseDate > DateTime.Now)) as List<Movie>;

            return View(soon_movies);
        }
        // Returns list of movies for Admin controller. The action and the view only for administration purpose
        // TODO Add Authorize attribute
        public async Task<ActionResult> MoviesList()
        {
            List<Movie> movies = (await db.Movies.GetAllAsync()) as List<Movie>; 
            return View(movies);
        }
        [AllowAnonymous]
        public async Task<ActionResult> MovieDetails(int? movieId)
        {
            if (movieId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(art => art.Id == movieId);
            if (movie == null)
            {
                return HttpNotFound();
            }
            //Move following code in corresponding Model          
            MovieSeances ms = new MovieSeances();
            ms.Movie = movie;      
            ICollection<Cinema> cinemas = await db.Cinemas.GetAllAsync();
            foreach(Cinema cinema in cinemas)
            {
                IEnumerable<Seance> seances_in_cinema = await db.Seances.FindAllAsync(seance => seance.Hall.CinemaId == cinema.Id );
                
                
                List<Seance> movie_seances = seances_in_cinema.Where(s => s.MovieId == movie.Id).OrderBy(s => s.Date).ToList<Seance>();
                IEnumerable<DateTime> seances_dates = movie_seances.Select(s => s.Date).Distinct().OrderBy(d => d.Date );
                
                CinemaSeances cs = new CinemaSeances();
                foreach (var date in seances_dates)
                {
                    DateSeances ds = new DateSeances();
                    ds.Date = date;
                    ds.Seances = movie_seances.Where(s => s.Date == date).ToList<Seance>(); //from seance in movie_seances
                                 //where s
                    cs.DateSeances.Add(ds);
                }
                //cs.CinemaName = cinema.Name;
                cs.Cinema = cinema;
                ms.Cinemas.Add(cs);

            }
            return View(ms);
        
        }
        //All code shown below is for administrator only
        [HttpGet]
        public ViewResult CreateMovie()
        {
            return View();
        }
        [HttpPost]
        public async  Task<ActionResult> CreateMovie([Bind(Include = "Name, ReleaseDate, Duration, Genre, Director, Scenario, Actors, Description")]
            Movie movie, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (image != null)
                {
                    using (var binaryReader = new BinaryReader(image.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(image.ContentLength);
                    }
                    movie.ImageMimeType = image.ContentType;
                    movie.ImageData = imageData;
                }
                await db.Movies.InsertAsync(movie);
                TempData["message"] = string.Format("{0} has been saved", movie.Name);
                return RedirectToAction("MoviesList");
            }
            else
            {
                TempData["message"] = string.Format("Movie hasn't been saved");
                return View(movie);
            }
        }

        [HttpGet]
        public async Task<ViewResult> EditMovie(int? movieId)
        {
            if (movieId == null)
            {
      //          return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(art => art.Id == movieId);
            if (movie == null)
            {
        //        return HttpNotFound();
            }

            return View(movie);
        }
        [HttpPost]
        public async Task<ActionResult> EditMovie([Bind(Include = "Id, Name, ReleaseDate, Duration, Genre, Director, Scenario, Actors, Description")]
            Movie movie, HttpPostedFileBase image = null)
        {
           if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (image != null)
                {
                    using (var binaryReader = new BinaryReader(image.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(image.ContentLength);
                    }
                    movie.ImageMimeType = image.ContentType;
                    movie.ImageData = imageData;
                }
                await db.Movies.EditAsync(movie, movie.Id);
                TempData["message"] = string.Format("{0} has been saved", movie.Name);
                return RedirectToAction("MoviesList");
            }
            else
            {
                TempData["message"] = string.Format("Movie hasn't been saved");
                return View(movie);
            }
        }
        [HttpPost]
        public async Task<ActionResult> DeleteMovie(int movieId)
        {
            Movie deletedMovie = await db.Movies.GetByIdAsync(movieId);
            if (deletedMovie != null)
            {
                await db.Movies.DeleteAsync(deletedMovie);
                TempData["message"] = string.Format("{0} was deleted", deletedMovie.Name);
            }
            return RedirectToAction("MoviesList");
        }
    }
}