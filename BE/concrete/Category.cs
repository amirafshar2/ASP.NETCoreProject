namespace BE.Concrete
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>Akzentfarbe der Kategorie (Hex), z. B. für Badges.</summary>
        public string Color { get; set; } = "#2F5BEA";
        public bool Status { get; set; } = true;
        public List<Blog> Blogs { get; set; } = new();
    }
}
