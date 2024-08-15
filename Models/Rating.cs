namespace MovieProject.Models
{
    public class Rating
    {
        public int Id { get; set; } 
        public Movie ?Name { get; set; }
        public int Score { get; set; }    
    }
}
