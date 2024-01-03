using Microsoft.AspNetCore.Identity;

namespace WebPulse_WebManager.Models
{
    public class Credential : Model
    {
        #region Properties
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int WebsiteId { get; set; }
        public required Website Website { get; set; }
        public virtual List<ApplicationUser> AssignedUsers { get; set; }
        public bool Valid { get; set; }

        #endregion

        #region Ctor

        public Credential() : base()
        {
            AssignedUsers = new List<ApplicationUser>();
            Description = string.Empty;
            Valid = true;
        }

        #endregion
    }
}
