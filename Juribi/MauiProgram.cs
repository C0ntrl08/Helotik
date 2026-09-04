using Microsoft.Extensions.Logging;
using Juribi.Services;
using Juribi.Validation;
using Juribi.ViewModels;
using Juribi.Views;

namespace Juribi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<JobApplicationValidator>();
            builder.Services.AddSingleton<IJobApplicationRepository, JobApplicationRepository>();

            builder.Services.AddTransient<JobApplicationListViewModel>();
            builder.Services.AddTransient<JobApplicationDetailViewModel>();
            builder.Services.AddTransient<JobApplicationEditViewModel>();

            builder.Services.AddTransient<MainPageView>();
            builder.Services.AddTransient<JobApplicationDetailPage>();
            builder.Services.AddTransient<JobApplicationEditPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
