using BE.Concrete;

namespace BLL.Dtos
{
    public class ChartPoint
    {
        public string Label { get; set; }
        public int Value { get; set; }
        public string Color { get; set; }
    }

    public class AdminDashboardDto
    {
        public int BlogCount { get; set; }
        public int PublishedCount { get; set; }
        public int WriterCount { get; set; }
        public int CommentCount { get; set; }
        public int PendingComments { get; set; }
        public int SubscriberCount { get; set; }
        public int UnreadContacts { get; set; }
        public int TotalViews { get; set; }
        public double AverageRating { get; set; }
        public List<ChartPoint> BlogsPerCategory { get; set; } = new();
        public List<ChartPoint> BlogsPerMonth { get; set; } = new();
        public List<ChartPoint> TopWriters { get; set; } = new();
        public List<Comment> LatestComments { get; set; } = new();
        public List<Contact> LatestContacts { get; set; } = new();
        public List<Blog> PopularBlogs { get; set; } = new();
    }

    public class WriterDashboardDto
    {
        public Writer Writer { get; set; }
        public int BlogCount { get; set; }
        public int PublishedCount { get; set; }
        public int CommentCount { get; set; }
        public int TotalViews { get; set; }
        public double AverageRating { get; set; }
        public int UnreadMessages { get; set; }
        public int CategoryCount { get; set; }
        public List<ChartPoint> ViewsPerBlog { get; set; } = new();
        public List<ChartPoint> BlogsPerCategory { get; set; } = new();
        public List<Blog> LatestBlogs { get; set; } = new();
        public List<Comment> LatestComments { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();
    }
}
