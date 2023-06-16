using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace Task10
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        }

        private IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServises);

        private void ConfigureServises(HostBuilderContext hostContext, IServiceCollection services) => services
            .AddViewModels()
            ;
        
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

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
