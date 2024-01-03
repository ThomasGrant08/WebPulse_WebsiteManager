namespace WebPulse_WebManager.Models
{
    public class Model
    {

        #region Properties
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
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
