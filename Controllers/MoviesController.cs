using Microsoft.AspNetCore.Mvc;
using MovieProject.DTO;
using MovieProject.Services;

namespace MovieProject.Controllers
{
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
        public async Task<ActionResult> Delete(int id)
        {
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