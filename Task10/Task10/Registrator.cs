using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Task10._TEST;
using Task10.Data;
using Task10.Models;
using Task10.Repository;
using Task10.Repository.Base;
using Task10.Repository.Interfaces;
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
            .AddRepisitories()
            ;

        public static IServiceCollection AddRepisitories(this IServiceCollection services) => services
            .AddTransient<IRepository<Course>, CoursesRepository>()
            .AddTransient<IRepository<Group>, RepositoryBase<Group>>()
            .AddTransient<IRepository<Student>, RepositoryBase<Student>>()
            .AddTransient<IRepository<Teacher>, RepositoryBase<Teacher>>()
            ;
    }
}
