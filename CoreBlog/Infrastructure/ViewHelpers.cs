using Microsoft.AspNetCore.Html;
using System.Text;

namespace CoreBlog.Infrastructure
{
    /// <summary>Kleine Hilfsfunktionen für die Razor-Views.</summary>
    public static class ViewHelpers
    {
        public static IHtmlContent Stars(double value, string label = null)
        {
            var full = (int)Math.Round(value);
            var sb = new StringBuilder($"<span class=\"stars\" role=\"img\" aria-label=\"{(label ?? $"{value:0.0} von 5 Sternen")}\">");
            for (int i = 1; i <= 5; i++)
                sb.Append(i <= full ? "<i class=\"fa-solid fa-star\"></i>" : "<i class=\"fa-solid fa-star off\"></i>");
            sb.Append("</span>");
            return new HtmlString(sb.ToString());
        }

        public static string Initial(string name) =>
            string.IsNullOrWhiteSpace(name) ? "?" : name.Trim()[0].ToString().ToUpperInvariant();

        public static string Relative(DateTime date)
        {
            var days = (DateTime.Today - date.Date).Days;
            return days switch
            {
                0 => date.TimeOfDay == TimeSpan.Zero ? "heute" : date > DateTime.Now.AddHours(-1) ? "gerade eben" : $"heute, {date:HH:mm} Uhr",
                1 => "gestern",
                < 7 => $"vor {days} Tagen",
                < 14 => "vor einer Woche",
                < 31 => $"vor {days / 7} Wochen",
                _ => date.ToString("d. MMMM yyyy")
            };
        }

        public static string Short(string text, int length) => ContentFormatter.Excerpt(text, length);

        public static string Slug(string title)
        {
            var s = (title ?? "").ToLowerInvariant()
                .Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("ß", "ss");
            s = System.Text.RegularExpressions.Regex.Replace(s, "[^a-z0-9]+", "-").Trim('-');
            return s.Length > 60 ? s[..60].TrimEnd('-') : s;
        }

        public static string Avatar(string image) => string.IsNullOrEmpty(image) ? "/img/avatar-default.svg" : image;
    }
}
