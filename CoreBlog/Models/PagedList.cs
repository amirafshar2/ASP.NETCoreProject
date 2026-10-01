namespace CoreBlog.Models
{
    /// <summary>Einfache Seitenaufteilung für Listen.</summary>
    public class PagedList<T>
    {
        public List<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;

        public PagedList(IEnumerable<T> source, int page, int pageSize)
        {
            var list = source.ToList();
            TotalCount = list.Count;
            PageSize = pageSize;
            Page = Math.Clamp(page, 1, Math.Max(1, (int)Math.Ceiling(TotalCount / (double)pageSize)));
            Items = list.Skip((Page - 1) * pageSize).Take(pageSize).ToList();
        }
    }

    public class PagerModel
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        /// <summary>Weitere Query-Parameter (z. B. Suche, Kategorie).</summary>
        public Dictionary<string, string> RouteValues { get; set; } = new();
    }
}
