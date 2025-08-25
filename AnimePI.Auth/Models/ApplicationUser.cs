using Microsoft.AspNetCore.Identity;

namespace AnimePI.Auth.Models;

public class ApplicationUser : IdentityUser
{
    public Guid UserId { get; set; }
}
