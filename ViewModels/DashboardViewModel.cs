using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.ViewModels
{
    public class DashboardIndexViewModel : SectionViewModel
    {
        #region Properties

        public List<Group> Groups { get; set; }

        #endregion

        #region Ctor 

        public DashboardIndexViewModel()
        {

        }

        #endregion
    }
}
