using WebPulse_WebManager.Enums;

namespace WebPulse_WebManager.Models
{
    public class SystemLog : Model
    {
        #region Properties

        public string EntityName { get; set; } = default!;
        public string EntityId { get; set; } = default!;
        public CrudActionType ActionType { get; set; } = default!;
        public required ApplicationUser User { get; set; }

        #endregion


    }
}
