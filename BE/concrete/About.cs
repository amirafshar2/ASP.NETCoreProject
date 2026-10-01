namespace BE.Concrete
{
    /// <summary>Inhalt der Seite "Über uns".</summary>
    public class About
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details1 { get; set; }
        public string Details2 { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string MapLocation { get; set; }
        public bool Status { get; set; } = true;
    }
}
