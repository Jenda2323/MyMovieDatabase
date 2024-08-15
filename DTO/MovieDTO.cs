using System.Diagnostics.CodeAnalysis;

namespace MovieProject.DTO
{
    public class MovieDTO
    {
        public int Id { get; set; }
        [NotNull]
        public string? Title { get; set; }
        [NotNull]
        public string? Director { get; set; }
        [NotNull]
        public int? ReleaseDate { get; set; }
        public string? Genre { get; set; }
    }
}
