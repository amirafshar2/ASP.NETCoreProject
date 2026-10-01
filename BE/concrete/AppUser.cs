using Microsoft.AspNetCore.Identity;

namespace BE.Concrete
{
    /// <summary>Benutzerkonto (ASP.NET Core Identity) – jeder Benutzer hat ein Autorenprofil.</summary>
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Writer Writer { get; set; }
    }
}
