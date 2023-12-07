using Microsoft.AspNetCore.Mvc;

namespace WebPulse_WebManager.ViewModels
{
    public class SectionViewModel
    {
        #region Properties

        public string SectionName { get; set; }

        public string SectionUrl { get; set; }

        #endregion

        #region Ctor

        public SectionViewModel()
        {
            SectionName = GetSectionName();
            SectionUrl = String.Format("/{0}/", GetSectionName());
        }

        #endregion

        #region Methods

        public string GetSectionName()
        {
            string className = this.GetType().Name;

            className = className.Replace("Index", string.Empty);
            className = className.Replace("Details", string.Empty);
            className = className.Replace("Edit", string.Empty);
            className = className.Replace("Create", string.Empty);
            className = className.Replace("Delete", string.Empty);

            return className.Replace("ViewModel", string.Empty);
        }

        #endregion


    }
}
