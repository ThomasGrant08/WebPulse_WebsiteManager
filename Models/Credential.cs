using Microsoft.AspNetCore.Identity;

namespace WebPulse_WebManager.Models
{
    public class Credential : Model
    {
        #region Properties
        public string Username { get; set; }
        public string Password { get; set; }

        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }

        public virtual List<ApplicationUser> AssignedUsers { get; set; }

        #endregion
    }
}
