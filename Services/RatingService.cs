using Microsoft.EntityFrameworkCore;
using MovieProject.DTO;
using MovieProject.Models;
using MovieProject.ViewModels;

namespace MovieProject.Services
{
    public class RatingService
    {
        private ApplicationDbContext _dbContext;

        public RatingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<RatingsDropdownViewModel> GetRatingsDropdownData()
        {
            var ratingsDropdownData = new RatingsDropdownViewModel()
            {
                Movies = await _dbContext.Movies.OrderBy(x => x.Title).ToListAsync(),
            };
            return ratingsDropdownData;
        }
        
        public async Task CreateAsync(RatingDTO ratingDTO)
        {
            var existingRating = await _dbContext.Ratings
                .FirstOrDefaultAsync(r => r.Name.Id == ratingDTO.MovieId);

            if (existingRating != null)
            {
               
                existingRating.Score = ratingDTO.Score;
                _dbContext.Ratings.Update(existingRating);
            }
            else
            {
                
                var newRating = new Rating
                {
                    Name = await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.Id == ratingDTO.MovieId),
                    Score = ratingDTO.Score
                };

                await _dbContext.Ratings.AddAsync(newRating);
            }

            await _dbContext.SaveChangesAsync();
        }
        private async Task<Rating> DtoToModel(RatingDTO ratingDTO)
        {
            return new Rating()
            {
                Name = await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.Id == ratingDTO.MovieId),
                Score = ratingDTO.Score,
                

            };
        }

        
        public async Task<IEnumerable<RatingsViewModel>> GetAllVMsAsync()
        {
            var ratings = await _dbContext.Ratings
                .Include(r => r.Name)
                .GroupBy(r => r.Name)
                .Select(g => new RatingsViewModel
                {
                    Id = g.First().Id,
                    MovieName = g.Key.Title,
                    AverageScore = Math.Round(g.Average(r => r.Score), 1)
                })
                .ToListAsync();

            return ratings;
        }
        internal async Task<Rating> GetByIdAsync(int id)
        {
            return await _dbContext.Ratings.Include(gr => gr.Name).FirstOrDefaultAsync(rating => rating.Id == id);
        }
        internal RatingDTO ModelToDTO(Rating rating)
        {
            return new RatingDTO
            {
                Id = rating.Id,
                MovieId = rating.Name.Id,
                Score = rating.Score,
            };
        }
        
        internal async Task UpdateAsync(int id, RatingDTO ratingDTO)
        {
            
            var ratingToUpdate = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == id);

            if (ratingToUpdate != null)
            {              
                ratingToUpdate.Name = await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.Id == ratingDTO.MovieId);
                ratingToUpdate.Score = ratingDTO.Score;              
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"Rating with ID {id} not found.");
            }
        }
        internal async Task DeleteAsync(int id)
        {
            var ratingToDlete = await _dbContext.Ratings.FirstOrDefaultAsync(rating => rating.Id == id);
            _dbContext.Ratings.Remove(ratingToDlete);
            await _dbContext.SaveChangesAsync();
        }



        
    }
}
