using System.Net;
using Danik.WebUI.Code.Domain;
using Danik.WebUI.Controllers;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    // Optional: configure session options here
    options.IdleTimeout = TimeSpan.FromMinutes(20);
        options.Cookie.IsEssential = true;
});



var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    if (Env.Current.EnvType==Env.Type.Dev) app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions());

    exceptionHandlerApp.Run(context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var lastError = exceptionHandlerPathFeature?.Error;

        switch (lastError)
        {
            case NotAuthenticatedException:
                {
                    var returnUrl = context.Request.Path.ToString().Contains("Login") ? "" : ("?returnUrl=" + WebUtility.UrlEncode(context.Request.Path.ToString()));
                    context.Response.Redirect("/Public/Login/" + returnUrl);
                    break;
                }
            case BusinessException exception:
                {
                    var message = exception.Message;
                    context.Response.Redirect("/Public/BusinessException/?msg=" + WebUtility.UrlEncode(message));
                    break;
                }
            default:
                context.Response.WriteAsync("<pre>" + lastError + "</pre>");
                context.Response.StatusCode = 500;
                break;
        }
        return Task.CompletedTask;
    });
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
