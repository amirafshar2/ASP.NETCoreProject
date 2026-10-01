using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;

namespace CoreBlog.Infrastructure
{
    /// <summary>
    /// Wandelt den Blog-Text sicher in HTML um: Absätze (Leerzeile), "## " Zwischenüberschriften,
    /// "- " Listen und `Code`. Alles wird vorher HTML-kodiert (Schutz vor XSS).
    /// </summary>
    public static class ContentFormatter
    {
        private static readonly Regex InlineCode = new("`([^`]+)`", RegexOptions.Compiled);

        public static IHtmlContent ToHtml(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return HtmlString.Empty;

            var html = new StringBuilder();
            var blocks = content.Replace("\r\n", "\n").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var raw in blocks)
            {
                var block = raw.Trim();
                if (block.Length == 0) continue;

                if (block.StartsWith("## "))
                {
                    var text = Inline(block[3..]);
                    html.Append($"<h2 id=\"{Slug(block[3..])}\">{text}</h2>");
                }
                else if (block.Split('\n').All(l => l.TrimStart().StartsWith("- ")))
                {
                    html.Append("<ul>");
                    foreach (var line in block.Split('\n'))
                        html.Append($"<li>{Inline(line.TrimStart()[2..])}</li>");
                    html.Append("</ul>");
                }
                else
                {
                    html.Append($"<p>{Inline(block).Replace("\n", "<br>")}</p>");
                }
            }
            return new HtmlString(html.ToString());
        }

        /// <summary>Zwischenüberschriften für das Inhaltsverzeichnis.</summary>
        public static List<(string Id, string Text)> Headings(string content) =>
            (content ?? "").Replace("\r\n", "\n").Split('\n')
                .Where(l => l.TrimStart().StartsWith("## "))
                .Select(l => l.Trim()[3..])
                .Select(t => (Slug(t), t))
                .ToList();

        public static string Excerpt(string text, int length)
        {
            if (string.IsNullOrEmpty(text)) return "";
            text = Regex.Replace(text.Replace("## ", ""), @"\s+", " ").Trim();
            if (text.Length <= length) return text;
            var cut = text.LastIndexOf(' ', length);
            return text[..(cut > 0 ? cut : length)] + " …";
        }

        private static string Inline(string text) =>
            InlineCode.Replace(WebUtility.HtmlEncode(text), "<code>$1</code>");

        private static string Slug(string text)
        {
            var s = text.ToLowerInvariant()
                .Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("ß", "ss");
            s = Regex.Replace(s, "[^a-z0-9]+", "-").Trim('-');
            return s.Length == 0 ? "abschnitt" : s;
        }
    }
}
