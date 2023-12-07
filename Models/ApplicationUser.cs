using Microsoft.AspNetCore.Identity;

namespace WebPulse_WebManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        #region Properties

        public virtual List<Group> Groups { get; set; }
        public virtual List<Credential> Credentials { get; set; }
        public virtual List<Website> Websites { get; set; }
        public bool GodView { get; set; }

        public byte[]? ProfilePicture { get; set; }

        #endregion

    }

    public class ApplicationRole : IdentityRole
    {

    }
}
