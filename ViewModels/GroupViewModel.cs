using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.ViewModels
{
    public class GroupIndexViewModel : SectionViewModel
    {
        #region Properties

        public List<Group> Groups { get; set; }

        #endregion

        #region Ctor 

        public GroupIndexViewModel()
        {
            SectionName = "My Groups";
        }

        #endregion
    }

    public class GroupViewModel : SectionViewModel
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ApplicationUser> AssignedUsers { get; set; } = new List<ApplicationUser>();

        public List<Website> Websites { get; set; } = new List<Website>();

        public List<Credential> Credentials { get; set; } = new List<Credential>();

        #endregion


    }

    public class GroupFormViewModel
    {
        #region Properties

        [Required]
        public string Name { get; set; }

        public string Description { get; set; } = string.Empty!;
        public IFormFile? Image { get; set; } = null!;

        #endregion
    }

}
