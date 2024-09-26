using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieProject.DTO;
using MovieProject.Services;

namespace MovieProject.Controllers

{
    [Authorize]
    public class MoviesController : Controller
    {
        private MovieService _movieService;

        public MoviesController(MovieService movieService)
        {
            _movieService = movieService;
        }

        public IActionResult Index()
        {
            IEnumerable<MovieDTO> allMovies = _movieService.GetMovies();
            return View(allMovies);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(MovieDTO movieDTO)
        {
            await _movieService.AddMovieAsync(movieDTO);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update(MovieDTO movieDTO, int id)
        {
            await _movieService.UpdateAsync(id, movieDTO);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var movieToEdit = await _movieService.GetByIdAsync(id);
            if (movieToEdit == null)
            {
                return View("NotFound");
            }
            return View(movieToEdit);
        }
        
        [HttpPost]
        
        public async Task<ActionResult> Delete(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Console.WriteLine("Uživatel není přihlášen.");
                return RedirectToAction("AccessDenied", "Account");
            }

            if (!User.IsInRole("Admin"))
            {
                Console.WriteLine("Uživatel nemá roli Admin.");
                return RedirectToAction("AccessDenied", "Account");
            }

            var movieToDelete = await _movieService.GetByIdAsync(id);
            if (movieToDelete == null)
            {
                return View("NotFound");
            }

            await _movieService.DeleteAsync(id);
            return RedirectToAction("Index");
        }



    }
}