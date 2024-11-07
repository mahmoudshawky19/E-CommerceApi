using Microsoft.AspNetCore.Identity;

namespace E_CommerceApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
    }
}
