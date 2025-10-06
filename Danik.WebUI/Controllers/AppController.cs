using Danik.WebUI.Code.ORM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using Danik.WebUI.Code.Domain;

namespace Danik.WebUI.Controllers;

public class AppController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var fromSession = GetFromSession<User?>("User");
        ViewData["User"] = fromSession;
        base.OnActionExecuting(context);
    }

    public T? GetFromSession<T>(string key)
    {
        var value = HttpContext.Session.GetString(key);
        return string.IsNullOrEmpty(value) ? default : JsonConvert.DeserializeObject<T>(value);
    }

    public void PutInSession(string key, object? subj)
    {
        if (subj == null)
            HttpContext.Session.Remove(key);
        else
            HttpContext.Session.SetString(key, subj.ToMinJSON());

    }

}

public class BusinessException : Exception
{
    public BusinessException(string? message) : base(message)
    {
    }
}
public class NotAuthenticatedException : Exception
{

}

public class PartController : AppController
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = GetFromSession<User?>("User");
        if (user == null) throw new NotAuthenticatedException();
        base.OnActionExecuting(context);
    }
}
public class AdmController : AppController
{
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = GetFromSession<User?>("User");
        if (user == null) throw new NotAuthenticatedException();
        if (!user.IsAdmin) throw new BusinessException("Access denied");
        base.OnActionExecuting(context);
    }

}