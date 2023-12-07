using WebPulse_WebManager.Data;

namespace WebPulse_WebManager.Utility
{
    public class MenuHelper
    {
        private readonly ApplicationDbContext _context;

        public MenuHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public string IsActive(string currentController, string targetController)
        {
            return currentController == targetController ? "active" : "";
        }
    }
}
