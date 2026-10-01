namespace BE.Concrete
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        /// <summary>Bewertung 1–5 Sterne.</summary>
        public int Score { get; set; }
        /// <summary>true = freigeschaltet und öffentlich sichtbar.</summary>
        public bool Status { get; set; } = true;

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
