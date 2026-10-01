using BE.Concrete;
using System.ComponentModel.DataAnnotations;

namespace CoreBlog.Models
{
    public class HomeViewModel
    {
        public Blog Featured { get; set; }
        public List<Blog> Secondary { get; set; } = new();
        public PagedList<Blog> Blogs { get; set; }
        public List<Blog> Popular { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Writer> Writers { get; set; } = new();
        public Category ActiveCategory { get; set; }
        public string Search { get; set; }
        public bool IsFiltered => ActiveCategory != null || !string.IsNullOrWhiteSpace(Search);
    }

    public class BlogDetailViewModel
    {
        public Blog Blog { get; set; }
        public List<Blog> Related { get; set; } = new();
        public List<(string Id, string Text)> Headings { get; set; } = new();
        public CommentForm Comment { get; set; } = new();
        public int WriterBlogCount { get; set; }
    }

    public class CommentForm
    {
        public int BlogId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Score { get; set; } = 5;
    }

    public class AuthorViewModel
    {
        public Writer Writer { get; set; }
        public List<Blog> Blogs { get; set; } = new();
        public int TotalViews { get; set; }
        public double AverageRating { get; set; }
    }

    public class AboutViewModel
    {
        public About About { get; set; }
        public List<Writer> Writers { get; set; } = new();
        public int BlogCount { get; set; }
        public int CommentCount { get; set; }
        public int CategoryCount { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Bitte geben Sie Ihre E-Mail-Adresse ein.")]
        [EmailAddress(ErrorMessage = "Bitte geben Sie eine gültige E-Mail-Adresse ein.")]
        [Display(Name = "E-Mail-Adresse")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihr Passwort ein.")]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }

        [Display(Name = "Angemeldet bleiben")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Bitte geben Sie Ihren Namen ein.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Der Name muss zwischen 2 und 50 Zeichen lang sein.")]
        [Display(Name = "Vor- und Nachname")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre E-Mail-Adresse ein.")]
        [EmailAddress(ErrorMessage = "Bitte geben Sie eine gültige E-Mail-Adresse ein.")]
        [Display(Name = "E-Mail-Adresse")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bitte wählen Sie ein Passwort.")]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bitte wiederholen Sie das Passwort.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Die Passwörter stimmen nicht überein.")]
        [Display(Name = "Passwort wiederholen")]
        public string ConfirmPassword { get; set; }
    }
}
