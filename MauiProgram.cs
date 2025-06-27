using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIMC.Services;

namespace SIMC
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

#if DEBUG
            builder.Logging.AddDebug();
#endif
            if (string.IsNullOrWhiteSpace(Image2Text.API_URL) || string.IsNullOrWhiteSpace(Image2Text.API_CHECKER))
            {
                var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

                if (config != null)
                {
                    if (string.IsNullOrWhiteSpace(Image2Text.API_URL))
                    {
                        Image2Text.API_URL = config["API_URL"] ?? string.Empty;
                    }
                    if (string.IsNullOrWhiteSpace(Image2Text.API_CHECKER))
                    {
                        Image2Text.API_CHECKER = config["API_CHECKER"] ?? string.Empty;
                    }
                }
            }

            return builder.Build();
        }
    }
}
