using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

    public class WebsiteViewModel : SectionViewModel
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public string Url { get; set; } = string.Empty!;
        public Group Group { get; set; } = null!;
        public List<Credential> Credentials { get; set; } = new List<Credential>();
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        #endregion
    }

    public class WebsiteFormViewModel
    {
        #region Properties

        [Required]
        public string Name { get; set; } = string.Empty!;

        [Required]
        public string Url { get; set; } = string.Empty!;

        [Required]
        public int GroupId { get; set; }

        #endregion
    }
}
