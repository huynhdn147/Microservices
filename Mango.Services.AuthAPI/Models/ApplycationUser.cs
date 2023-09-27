using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Models
{
    public class ApplycationUser: IdentityUser
    {
        public string Name { set; get; }
    }
}
