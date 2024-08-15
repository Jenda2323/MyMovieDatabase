using MovieProject.Models;

namespace MovieProject.ViewModels
{
    public class RatingsDropdownViewModel
    {
        public List<Movie>Movies { get; set; }
        public RatingsDropdownViewModel()
        {
            Movies = new List<Movie>(); 
        }
    }
}
