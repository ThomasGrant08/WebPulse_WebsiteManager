using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebPulse_WebManager.Data;

namespace WebPulse_WebManager.Utility
{
    public static class NavigationUtilities
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string actions, string controllers, string cssClass = "active")
        {
            var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
            var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;

            var acceptedControllers = (controllers ?? currentController ?? "").Split(',');
            var acceptedActions = (actions ?? currentAction ?? "").Split(',');

            return acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction)
                ? cssClass
                : "";
        }

        public static string IsActive(this IHtmlHelper htmlHelper, string? controllers = null, string cssClass = "active")
        {
            var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;

            var acceptedControllers = (controllers ?? currentController ?? "").Split(',');

            return acceptedControllers.Contains(currentController)
                ? cssClass
                : "";
        }
    }
}
