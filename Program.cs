using BlazorApp1.Components;

namespace DatabaseWork
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                           .AddInteractiveServerComponents();

            // Register OracleService, API controller
            builder.Services.AddScoped<OracleService>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAntiforgery(); // Add antiforgery middleware
            app.MapControllers(); // Map API controllers
            app.MapRazorComponents<App>()
               .AddInteractiveServerRenderMode();

            await app.RunAsync();
        }
    }
}
