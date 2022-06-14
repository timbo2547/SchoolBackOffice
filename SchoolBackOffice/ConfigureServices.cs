using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Services;

namespace SchoolBackOffice
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebUiServices(this IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddServerSideBlazor();
            services.AddControllersWithViews();
            services.AddScoped<DialogService>();
            return services;
        }
    }
}