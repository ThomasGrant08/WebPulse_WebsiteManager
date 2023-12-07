using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebPulse_WebManager.Models
{
    public class Website : Model
    {

        #region Properties
        public string Name { get; set; }
        public string Url { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public virtual List<Credential> Credentials { get; set; } 

        public virtual List<ApplicationUser> Users { get; set; }

        #endregion

    }
}
