using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Task10._TEST;
using Task10.Data;
using Task10.Models;
using Task10.Services;
using Task10.Services.Base;
using Task10.Services.Interfaces;
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
            .AddDbServices()
            ;

        public static IServiceCollection AddDbServices(this IServiceCollection services) => services
            .AddTransient<IDbService<Course>, CoursesDbService>()
            .AddTransient<IDbService<Group>, DbServiceBase<Group>>()
            .AddTransient<IDbService<Student>, DbServiceBase<Student>>()
            .AddTransient<IDbService<Teacher>, DbServiceBase<Teacher>>()
            ;
    }
}
