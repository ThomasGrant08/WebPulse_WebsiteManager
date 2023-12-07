using Microsoft.AspNetCore.Identity;

namespace WebPulse_WebManager.Models
{
    public class Group : Model
    {
        #region Properties
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual List<ApplicationUser> AssignedUsers { get; set; } = new List<ApplicationUser>();

        public virtual List<Website> Websites { get; set; } = new List<Website>();

        #endregion
    }
}
