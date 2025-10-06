using Danik.WebUI.Code.ORM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Danik.WebUI.Controllers;

public class AppController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var fromSession = GetFromSession<string?>("Admin");
        ViewData["Admin"] = fromSession;
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

public class AdmController : AppController
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (GetFromSession<string?>("Admin") == null) throw new NotAuthenticatedException();
        base.OnActionExecuting(context);
    }

}