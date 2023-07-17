﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Task10.Data;
using Task10.Models;
using Task10.Services;
using Task10.Services.DbServices;
using Task10.Services.DbServices.Interfaces;
using Task10.Services.Interfaces;
using Task10.ViewModels;

namespace Task10
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