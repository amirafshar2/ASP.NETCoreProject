using BE.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CoreBlog.Models
{
    public class BlogEditViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public bool Status { get; set; } = true;
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public List<SelectListItem> Categories { get; set; } = new();
        public bool IsNew => Id == 0;
    }

    public class ProfileViewModel
    {
        public string Name { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool IsDemoAccount { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Bitte geben Sie Ihr aktuelles Passwort ein.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie ein neues Passwort ein.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Bitte wiederholen Sie das neue Passwort.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Die Passwörter stimmen nicht überein.")]
        public string ConfirmPassword { get; set; }
    }

    public class ComposeMessageViewModel
    {
        public int? ReceiverId { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public List<SelectListItem> Receivers { get; set; } = new();
    }

    public class MessageListViewModel
    {
        public string Box { get; set; } = "inbox";
        public List<Message> Messages { get; set; } = new();
        public int UnreadCount { get; set; }
        public int SentCount { get; set; }
        public int InboxCount { get; set; }
    }

    public class WriterAdminViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDemoAccount { get; set; }
        public int BlogCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AboutEditViewModel
    {
        public About About { get; set; }
        public IFormFile Image1File { get; set; }
        public IFormFile Image2File { get; set; }
    }
}
