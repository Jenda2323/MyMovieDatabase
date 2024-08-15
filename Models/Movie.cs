﻿namespace MovieProject.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string ?Title { get; set; }
        public string ?Director { get; set; }
        public int ?ReleaseDate { get; set; }
        public string ?Genre { get; set; }
    }

}