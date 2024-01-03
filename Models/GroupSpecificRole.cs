using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebPulse_WebManager.Models
{
    public class GroupSpecificRole : Model
    {
        #region Properties
        public required string Name { get; set; }
        public string Description { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }
        public required Group Group { get; set; }

        #endregion


        #region Ctor

        public GroupSpecificRole() : base() 
        {
            Description = string.Empty;
            Users = new List<ApplicationUser>();
        }   

        #endregion
    }
}
