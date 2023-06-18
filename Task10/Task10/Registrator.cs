using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Task10._TEST;
using Task10.Data;
using Task10.ViewModels;

namespace Task10
{
    internal static class Registrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>()
            ;

        public static IServiceCollection AddDatabase(this IServiceCollection services) => services
            .AddDbContext<Task10DbContext>(options =>
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Task10.db");
                options.UseSqlite($"Filename={dbPath}");
            })
            .AddTransient<DbInitializer>()
            ;
    }
}
