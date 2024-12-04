using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MetersMVC;

public class RequireAuthorizationAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {


        var actionName = context.ActionDescriptor.DisplayName;

        // Log the action name for debugging
        Console.WriteLine($"Action Name: {actionName}");

        // Skip the authorization check for the Login action
        if (actionName.Contains("Login", StringComparison.OrdinalIgnoreCase))
        {
            return; // Skip authorization check for Login action
        }
        // Skip authorization logic if the current action is "AuthError"
        if (actionName.Contains("AuthError"))
        {
            return;  // Don't apply the authorization check if it's the AuthError page
        }

        var token = context.HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            // Redirect only if the token is missing
            context.Result = new RedirectToActionResult("AuthError", "Home", new { message = "Authorization Required" });
            return;
        }

        // Check if the action has the AuthorizePermission attribute
        var hasAuthorizePermissionAttribute = context.ActionDescriptor.EndpointMetadata
            .OfType<AuthorizePermission>()
            .Any();

        if (!hasAuthorizePermissionAttribute)
        {
            context.Result = new RedirectToActionResult("AuthError", "Home", new { message = "Authorization Required" });
        }
    }



    //public void OnActionExecuting(ActionExecutingContext context)
    //{

    //    var token = context.HttpContext.Session.GetString("JwtToken");

    //    if (string.IsNullOrEmpty(token))
    //    {
    //        // Redirect only if the token is missing, otherwise let the AuthorizePermission attribute handle it
    //        context.Result = new RedirectToActionResult("AuthError", "Home", new { message = "Authorization Required" });
    //        return;
    //    }

    //    // Check if the action has the AuthorizePermission attribute
    //    var hasAuthorizePermissionAttribute = context.ActionDescriptor.EndpointMetadata
    //        .OfType<AuthorizePermission>()
    //        .Any();

    //    if (!hasAuthorizePermissionAttribute)
    //    {
    //        context.Result = new RedirectToActionResult("AuthError", "Home", new { message = "Authorization Required" });
    //    }
    //}

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}
