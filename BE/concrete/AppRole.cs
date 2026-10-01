using Microsoft.AspNetCore.Identity;

namespace BE.Concrete
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole() { }
        public AppRole(string name) : base(name) { }
    }
}
