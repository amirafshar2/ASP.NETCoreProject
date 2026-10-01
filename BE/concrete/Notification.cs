namespace BE.Concrete
{
    /// <summary>Hinweis, den das Admin-Team für alle Autoren veröffentlicht.</summary>
    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; }
        /// <summary>Font-Awesome-Klasse, z. B. "fa-solid fa-bullhorn".</summary>
        public string TypeSymbol { get; set; }
        public string SymbolColor { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool Status { get; set; } = true;
    }
}
