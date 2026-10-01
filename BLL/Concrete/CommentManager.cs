using BE.Concrete;
using BLL.Abstract;
using DAL.Abstract;

namespace BLL.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly ICommentDAL _commentDal;
        private readonly IBlogRatingDAL _ratingDal;

        public CommentManager(ICommentDAL commentDal, IBlogRatingDAL ratingDal)
        {
            _commentDal = commentDal;
            _ratingDal = ratingDal;
        }

        public void Add(Comment t)
        {
            _commentDal.Insert(t);
            RecalculateRating(t.BlogId);
        }

        public void Update(Comment t)
        {
            _commentDal.Update(t);
            RecalculateRating(t.BlogId);
        }

        public void Delete(Comment t)
        {
            var blogId = t.BlogId;
            _commentDal.Delete(t);
            RecalculateRating(blogId);
        }

        public Comment GetById(int id) => _commentDal.GetById(id);
        public List<Comment> GetList() => _commentDal.GetListAll();
        public int Count() => _commentDal.Count();
        public List<Comment> GetListWithBlog() => _commentDal.GetListWithBlog();
        public List<Comment> GetListByWriter(int writerId) => _commentDal.GetListByWriter(writerId);

        public void ToggleStatus(int id)
        {
            var c = _commentDal.GetById(id);
            if (c == null) return;
            c.Status = !c.Status;
            Update(c);
        }

        /// <summary>Berechnet die Bewertung eines Blogs aus allen freigeschalteten Kommentaren neu.</summary>
        private void RecalculateRating(int blogId)
        {
            var approved = _commentDal.GetListAll(c => c.BlogId == blogId && c.Status && c.Score > 0);
            var rating = _ratingDal.GetByBlogId(blogId);
            if (rating == null)
            {
                rating = new BlogRating { BlogId = blogId };
                _ratingDal.Insert(rating);
            }
            rating.TotalScore = approved.Sum(c => c.Score);
            rating.RatingCount = approved.Count;
            _ratingDal.Update(rating);
        }
    }
}
