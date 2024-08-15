using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieProject.DTO;
using MovieProject.Services;

namespace MovieProject.Controllers
{
    public class RatingsController : Controller
    {
        private RatingService _ratingService;

        public RatingsController(RatingService ratingService)
        {
            _ratingService = ratingService;
        }
        private async Task FillSelectectAsync()
        {
            var ratingsDropdownData = await _ratingService.GetRatingsDropdownData();
            ViewBag.Ratings = new SelectList(ratingsDropdownData.Movies, "Id", "Title");
        }
        public async Task<IActionResult> CreateAsync()
        {
            await FillSelectectAsync(); 
            return View();
        }
        
        public async Task<IActionResult> Index()
        {
            var allRatings=await _ratingService.GetAllVMsAsync();
            return View(allRatings);
        }
        [HttpPost]
        public async Task<IActionResult>Create(RatingDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                await FillSelectectAsync();
                return View(ratingDTO);
            }
            await _ratingService.CreateAsync(ratingDTO);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var ratingToEdit = await _ratingService.GetByIdAsync(id);
            if (ratingToEdit == null)
            {
                return View("NotFound");
            }
            var ratingDto = _ratingService.ModelToDTO(ratingToEdit);
            await FillSelectectAsync();
            return View(ratingDto);
        }
        [HttpPost]
        public async Task<IActionResult>Update(int id,RatingDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                await FillSelectectAsync();
                return View(ratingDTO);
            }
            await _ratingService.UpdateAsync(id, ratingDTO);  
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>DeleteAsync(int id)
        {
            await _ratingService.DeleteAsync(id);
            return RedirectToAction("Index");   
        }
    }
}
