using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string FullName { get; set; }
    public string ProfilePicture { get; set; }
}
