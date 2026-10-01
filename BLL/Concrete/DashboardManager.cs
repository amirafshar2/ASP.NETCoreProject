using BLL.Abstract;
using BLL.Dtos;
using System.Globalization;

namespace BLL.Concrete
{
    /// <summary>Bereitet Kennzahlen und Diagrammdaten für die Dashboards auf.</summary>
    public class DashboardManager : IDashboardService
    {
        private static readonly CultureInfo German = new("de-DE");

        private readonly IBlogService _blogs;
        private readonly ICategoryService _categories;
        private readonly ICommentService _comments;
        private readonly IWriterService _writers;
        private readonly IContactService _contacts;
        private readonly INewsLetterService _newsLetters;
        private readonly IMessageService _messages;
        private readonly INotificationService _notifications;

        public DashboardManager(IBlogService blogs, ICategoryService categories, ICommentService comments,
            IWriterService writers, IContactService contacts, INewsLetterService newsLetters,
            IMessageService messages, INotificationService notifications)
        {
            _blogs = blogs;
            _categories = categories;
            _comments = comments;
            _writers = writers;
            _contacts = contacts;
            _newsLetters = newsLetters;
            _messages = messages;
            _notifications = notifications;
        }

        public AdminDashboardDto GetAdminDashboard()
        {
            var blogs = _blogs.GetListWithDetails();
            var comments = _comments.GetListWithBlog();
            var rated = blogs.Where(b => b.Rating != null && b.Rating.RatingCount > 0).ToList();

            return new AdminDashboardDto
            {
                BlogCount = blogs.Count,
                PublishedCount = blogs.Count(b => b.Status),
                WriterCount = _writers.Count(),
                CommentCount = comments.Count,
                PendingComments = comments.Count(c => !c.Status),
                SubscriberCount = _newsLetters.Count(),
                UnreadContacts = _contacts.UnreadCount(),
                TotalViews = blogs.Sum(b => b.ViewCount),
                AverageRating = rated.Count == 0 ? 0 : Math.Round(
                    (double)rated.Sum(b => b.Rating.TotalScore) / rated.Sum(b => b.Rating.RatingCount), 1),
                BlogsPerCategory = _categories.GetListWithBlogCount()
                    .Select(c => new ChartPoint { Label = c.Name, Value = c.Blogs.Count, Color = c.Color })
                    .ToList(),
                BlogsPerMonth = LastMonths(6)
                    .Select(m => new ChartPoint
                    {
                        Label = m.ToString("MMM yy", German),
                        Value = blogs.Count(b => b.CreateDate.Year == m.Year && b.CreateDate.Month == m.Month)
                    }).ToList(),
                TopWriters = blogs.GroupBy(b => b.Writer?.Name ?? "–")
                    .Select(g => new ChartPoint { Label = g.Key, Value = g.Sum(b => b.ViewCount) })
                    .OrderByDescending(p => p.Value).Take(5).ToList(),
                LatestComments = comments.Take(5).ToList(),
                LatestContacts = _contacts.GetList().Take(4).ToList(),
                PopularBlogs = blogs.OrderByDescending(b => b.ViewCount).Take(5).ToList()
            };
        }

        public WriterDashboardDto GetWriterDashboard(int writerId)
        {
            var blogs = _blogs.GetListByWriter(writerId);
            var comments = _comments.GetListByWriter(writerId);
            var rated = blogs.Where(b => b.Rating != null && b.Rating.RatingCount > 0).ToList();

            return new WriterDashboardDto
            {
                Writer = _writers.GetById(writerId),
                BlogCount = blogs.Count,
                PublishedCount = blogs.Count(b => b.Status),
                CommentCount = comments.Count,
                TotalViews = blogs.Sum(b => b.ViewCount),
                AverageRating = rated.Count == 0 ? 0 : Math.Round(
                    (double)rated.Sum(b => b.Rating.TotalScore) / rated.Sum(b => b.Rating.RatingCount), 1),
                UnreadMessages = _messages.UnreadCount(writerId),
                CategoryCount = blogs.Select(b => b.CategoryId).Distinct().Count(),
                ViewsPerBlog = blogs.OrderByDescending(b => b.ViewCount).Take(6)
                    .Select(b => new ChartPoint
                    {
                        Label = b.Title.Length > 28 ? b.Title[..26] + "…" : b.Title,
                        Value = b.ViewCount
                    }).ToList(),
                BlogsPerCategory = blogs.GroupBy(b => b.CategoryId)
                    .Select(g => new ChartPoint { Label = g.First().Category.Name, Value = g.Count(), Color = g.First().Category.Color })
                    .ToList(),
                LatestBlogs = blogs.Take(5).ToList(),
                LatestComments = comments.Take(5).ToList(),
                Notifications = _notifications.GetActiveList().Take(4).ToList()
            };
        }

        private static IEnumerable<DateTime> LastMonths(int count)
        {
            var start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            for (int i = count - 1; i >= 0; i--)
                yield return start.AddMonths(-i);
        }
    }
}
