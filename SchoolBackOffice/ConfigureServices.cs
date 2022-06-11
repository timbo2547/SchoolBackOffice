using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SchoolBackOffice.Infrastructure.Persistence;

namespace SchoolBackOffice
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebUiServices(this IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddServerSideBlazor();
            services.AddControllersWithViews();
            services.AddScoped<DialogService>();
            return services;
        }
    }
}