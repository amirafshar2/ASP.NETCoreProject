using BE.Concrete;
using BLL.Abstract;
using DAL.Abstract;

namespace BLL.Concrete
{
    public class BlogManager : IBlogService
    {
        private readonly IBlogDAL _blogDal;
        private readonly IBlogRatingDAL _ratingDal;

        public BlogManager(IBlogDAL blogDal, IBlogRatingDAL ratingDal)
        {
            _blogDal = blogDal;
            _ratingDal = ratingDal;
        }

        public void Add(Blog t)
        {
            _blogDal.Insert(t);
            // Jeder Blog bekommt einen Bewertungsdatensatz (früher per SQL-Trigger)
            _ratingDal.Insert(new BlogRating { BlogId = t.Id });
        }

        public void Update(Blog t) => _blogDal.Update(t);
        public void Delete(Blog t) => _blogDal.Delete(t);
        public Blog GetById(int id) => _blogDal.GetById(id);
        public List<Blog> GetList() => _blogDal.GetListAll();
        public int Count() => _blogDal.Count();

        public List<Blog> GetPublishedList() => _blogDal.GetListWithDetails(onlyPublished: true);
        public List<Blog> GetListWithDetails() => _blogDal.GetListWithDetails(onlyPublished: false);
        public Blog GetDetail(int id) => _blogDal.GetByIdWithDetails(id);
        public List<Blog> GetListByWriter(int writerId) => _blogDal.GetListByWriter(writerId);
        public List<Blog> GetLatest(int count) => GetPublishedList().Take(count).ToList();

        public List<Blog> GetPopular(int count) => GetPublishedList()
            .OrderByDescending(b => b.ViewCount).Take(count).ToList();

        public Blog GetFeatured()
        {
            var list = GetPublishedList();
            return list.FirstOrDefault(b => b.IsFeatured) ?? list.FirstOrDefault();
        }

        public List<Blog> GetRelated(Blog blog, int count) => GetPublishedList()
            .Where(b => b.Id != blog.Id)
            .OrderByDescending(b => b.CategoryId == blog.CategoryId)
            .ThenByDescending(b => b.CreateDate)
            .Take(count).ToList();

        public List<Blog> Search(string term, int? categoryId) => _blogDal.Search(term, categoryId);

        public void RegisterView(int id) => _blogDal.IncreaseViewCount(id);

        public void ToggleStatus(int id)
        {
            var blog = _blogDal.GetById(id);
            if (blog == null) return;
            blog.Status = !blog.Status;
            _blogDal.Update(blog);
        }

        public void SetFeatured(int id)
        {
            foreach (var b in _blogDal.GetListAll(x => x.IsFeatured))
            {
                var tracked = _blogDal.GetById(b.Id);
                tracked.IsFeatured = false;
                _blogDal.Update(tracked);
            }
            var blog = _blogDal.GetById(id);
            if (blog == null) return;
            blog.IsFeatured = true;
            _blogDal.Update(blog);
        }
    }
}
