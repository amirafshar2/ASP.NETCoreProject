namespace BE.Concrete
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        /// <summary>Kurzer Teaser für Listen und Vorschau.</summary>
        public string Summary { get; set; }
        /// <summary>Inhalt: Absätze durch Leerzeilen getrennt, "## " für Zwischenüberschriften.</summary>
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool Status { get; set; } = true;
        public bool IsFeatured { get; set; }
        public int ViewCount { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int WriterId { get; set; }
        public Writer Writer { get; set; }

        public List<Comment> Comments { get; set; } = new();
        public BlogRating Rating { get; set; }

        /// <summary>Geschätzte Lesezeit in Minuten (ca. 200 Wörter pro Minute).</summary>
        public int ReadingMinutes =>
            Math.Max(1, (int)Math.Ceiling((Content ?? "").Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Length / 200.0));
    }
}
