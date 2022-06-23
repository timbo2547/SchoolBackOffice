using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SchoolBackOffice.Application.Common.Interfaces;
using SchoolBackOffice.Services;
using SchoolBackOffice.Services.StaffUsers;

namespace SchoolBackOffice
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebUiServices(this IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IStaffUserViewModelService, StaffUserViewModelService>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllersWithViews();
            services.AddScoped<DialogService>();
            return services;
        }
    }
}