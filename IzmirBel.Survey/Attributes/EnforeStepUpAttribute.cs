
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class EnforeStepUpAttribute : Attribute, IAuthorizationFilter
{
    public const string StepUpAllowPathName = "StepUpAllowPath";
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var email = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        var stepUpAllowPath = context.HttpContext.Session.GetString(email + StepUpAllowPathName)?.ToLower();

        if (!string.IsNullOrEmpty(stepUpAllowPath))
        {
            context.HttpContext.Session.Remove(email + StepUpAllowPathName);

            if (context.HttpContext.Request.Path.ToString().ToLower().Equals(stepUpAllowPath))
                return;

            context.HttpContext.Response.Redirect("/Identity/Account/Check2fa?ReturnUrl=" + context.HttpContext.Request.Path);
        }
        else
        {
            context.HttpContext.Response.Redirect("/Identity/Account/Check2fa?ReturnUrl=" + context.HttpContext.Request.Path);
        }
    }
}

