namespace WebPulse_WebManager.Models
{
    public class Invitation : Model
    {
        #region Properties

        public required ApplicationUser InvitedBy { get; set; }

        public required ApplicationUser InvitedUser { get; set; }

        public required Group Group { get; set; }

        public bool Accepted { get; set; } = false;
        #endregion

    }
}
