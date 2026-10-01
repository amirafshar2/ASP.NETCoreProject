namespace BE.Concrete
{
    /// <summary>Interne Nachricht zwischen Autoren.</summary>
    public class Message
    {
        public int Id { get; set; }
        public int? SenderId { get; set; }
        public Writer Sender { get; set; }
        public int? ReceiverId { get; set; }
        public Writer Receiver { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }
    }
}
