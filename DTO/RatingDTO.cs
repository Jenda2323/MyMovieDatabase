using MovieProject.Models;

namespace MovieProject.DTO
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int Score { get; set; }
    }
}
