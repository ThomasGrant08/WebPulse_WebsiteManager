namespace WebPulse_WebManager.Models
{
    public class Model
    {

        #region Properties
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }

        #endregion

        #region Ctor

        public Model()
        {
            CreatedAt = DateTime.Now;
            LastUpdatedAt = DateTime.Now;
        }

        #endregion
    }
}
