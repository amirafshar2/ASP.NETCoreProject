namespace BE.Concrete
{
    public class NewsLetter
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool Status { get; set; } = true;
    }
}
