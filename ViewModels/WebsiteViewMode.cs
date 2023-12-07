using Microsoft.AspNetCore.Mvc;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.ViewModels
{
    public class WebsiteIndexViewMode : SectionViewModel
    {
        #region Properties 

        public List<Website> Websites { get; set; }

        #endregion

        #region Ctor

        public WebsiteIndexViewMode()
        {
            SectionName = "My Websites";
        }

        #endregion 

    }
}
