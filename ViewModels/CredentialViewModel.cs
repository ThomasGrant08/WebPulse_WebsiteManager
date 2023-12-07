using Microsoft.AspNetCore.Mvc;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.ViewModels
{
    public class CredentialsIndexViewModel : SectionViewModel
    {
        #region Properties

        public List<Credential> Credentials { get; set; }

        #endregion

        #region Ctor

        public CredentialsIndexViewModel()
        {
            SectionName = "My Credentials";
        }

        #endregion

    }
}
