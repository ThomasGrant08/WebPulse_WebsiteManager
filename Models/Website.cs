using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebPulse_WebManager.Models
{
    public class Website : Model
    {

        #region Properties
        public required string Name { get; set; }
        public required string Url { get; set; }

        public int GroupId { get; set; }
        public required Group Group { get; set; }
        public virtual List<Credential> Credentials { get; set; } 

        public virtual List<ApplicationUser> Users { get; set; }

        #endregion


        #region Ctor
        public Website() : base()
        {
            Credentials = new List<Credential>();
            Users = new List<ApplicationUser>();
        }

        #endregion

    }
}
