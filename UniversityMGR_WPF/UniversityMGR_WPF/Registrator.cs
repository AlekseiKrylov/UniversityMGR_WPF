using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using UniversityMGR_WPF.Data;
using UniversityMGR_WPF.Models;
using UniversityMGR_WPF.Services;
using UniversityMGR_WPF.Services.DbServices;
using UniversityMGR_WPF.Services.DbServices.Interfaces;
using UniversityMGR_WPF.Services.Interfaces;
using UniversityMGR_WPF.ViewModels;
using UniversityMGR_WPF.ViewModels.UserDialog;

namespace UniversityMGR_WPF
{
    internal static class Registrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>()
            .AddTransient<CoursesAndGroupsViewModel>()
            .AddTransient<StudentsViewModel>()
            .AddTransient<TeachersViewModel>()
            .AddTransient<CourseEditorViewModel>()
            .AddTransient<GroupEditorViewModel>()
            .AddTransient<StudentEditorViewModel>()
            .AddTransient<TeacherEditorViewModel>()
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
            .AddTransient<IDbService<Group>, GroupsDbService>()
            .AddTransient<IDbService<Student>, StudentsDbService>()
            .AddTransient<IDbService<Teacher>, TeachersDbService>()
            .AddTransient<IUserDialogService, WindowsUserDialogService>()
            .AddTransient<IFileService, FileService>()
            ;
    }
}
