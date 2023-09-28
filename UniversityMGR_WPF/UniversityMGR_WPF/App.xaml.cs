using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Windows;
using UniversityMGR_WPF;
using UniversityMGR_WPF.Data;

namespace Task10
{
    public partial class App : Application
    {
        public static Window ActiveWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);
        public static IHost? AppHost { get; private set; }

        public App() => AppHost = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServises);

        private static void ConfigureServises(HostBuilderContext hostContext, IServiceCollection services) => services
            .AddDatabase()
            .AddViewModels()
            ;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var scope = AppHost!.Services.CreateScope())
                scope.ServiceProvider.GetRequiredService<DbInitializer>().DbInitializeAsync().Wait();

            await AppHost!.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            using (AppHost)
                await AppHost!.StopAsync();

            AppHost = null;
        }
    }
}
