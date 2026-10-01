namespace BE.Concrete
{
    /// <summary>Aggregierte Bewertung eines Blogs (ersetzt die früheren SQL-Server-Trigger).</summary>
    public class BlogRating
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public int TotalScore { get; set; }
        public int RatingCount { get; set; }

        public double Average => RatingCount == 0 ? 0 : Math.Round((double)TotalScore / RatingCount, 1);
    }
}
