using Microsoft.EntityFrameworkCore;
using MovieProject.DTO;
using MovieProject.Models;

namespace MovieProject.Services
{
    public class MovieService
    {
        private ApplicationDbContext _dbContext;

        public MovieService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<MovieDTO> GetMovies()
        {
            var allmovies = _dbContext.Movies;
            var movieDtos = new List<MovieDTO>();
            foreach (var movie in allmovies)
            {
                movieDtos.Add(ModeltoDto(movie));

            }
            return movieDtos;
        }


        public async Task AddMovieAsync(MovieDTO movie)
        {

            await _dbContext.Movies.AddAsync(
                DtoToModel(movie)
            );
            await _dbContext.SaveChangesAsync();
        }

        private static Movie DtoToModel(MovieDTO movie)
        {
            return new Movie
            {
                Title = movie.Title,
                Director = movie.Director,
                Genre = movie.Genre,
                ReleaseDate = movie.ReleaseDate,
                Id = movie.Id, 
            };
        }

        internal async Task<MovieDTO> GetByIdAsync(int id)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
            if (movie == null)
            {
                return null;
            }
            return ModeltoDto(movie);
        }
        private static MovieDTO ModeltoDto(Movie movie)
        {
            return new MovieDTO
            {
                Title = movie.Title,
                Director = movie.Director,
                Genre = movie.Genre,
                ReleaseDate = movie.ReleaseDate,
                Id = movie.Id,
            };
        }

        internal async Task UpdateAsync(int id, MovieDTO movieDTO)
        {
            _dbContext.Movies.Update(DtoToModel(movieDTO));
            await _dbContext.SaveChangesAsync();
                                                                        
        }
        internal async Task DeleteAsync(int id)
        {
            var movieToDelete = await _dbContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
            
            _dbContext.Movies.Remove(movieToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
