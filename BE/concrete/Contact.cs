namespace BE.Concrete
{
    /// <summary>Nachricht aus dem Kontaktformular.</summary>
    public class Contact
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        /// <summary>true = gelesen.</summary>
        public bool IsRead { get; set; }
    }
}
