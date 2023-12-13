using Microsoft.AspNetCore.Identity;

namespace WebPulse_WebManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        #region Properties

        public int usernameSuffix { get; set; }

        public virtual List<Group> Groups { get; set; }
        public virtual List<Credential> Credentials { get; set; }
        public virtual List<Website> Websites { get; set; }
        public bool GodView { get; set; }

        public byte[]? ProfilePicture { get; set; }

        #endregion

        #region Ctor 

        public ApplicationUser()
        {
            
        }

        #endregion

        #region Methods

        public string GetUsername()
        {
            var username = this.UserName;
            var suffix = this.usernameSuffix;

            return String.Format("{0}#{1}", username, suffix);
        }

        #endregion
    }

    public class ApplicationRole : IdentityRole
    {
    }
}
