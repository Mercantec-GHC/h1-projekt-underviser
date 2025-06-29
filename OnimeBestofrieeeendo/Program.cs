using OnimeBestofrieeeendo.Components;
using OnimeBestofrieeeendo.Components.Services;

namespace OnimeBestofrieeeendo;

public class Program

{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddScoped<ContactService>();
        builder.Services.AddScoped<UserProfileService>();
        builder.Services.AddScoped<DbService>();
        builder.Services.AddScoped<ShopService>();
        builder.Services.AddScoped<ShopBusinessService>();
        builder.Services.AddScoped<ErrorHandler>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}