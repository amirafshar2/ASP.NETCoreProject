namespace BE.Concrete
{
    /// <summary>Autorenprofil – verknüpft mit einem Benutzerkonto.</summary>
    public class Writer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string Mail { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public List<Blog> Blogs { get; set; } = new();
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
}
