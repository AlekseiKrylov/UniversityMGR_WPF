using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Task10.Data;
using Task10.Models;

namespace Task10._TEST
{
    internal class DbInitializer
    {
        private readonly Task10DbContext _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(Task10DbContext db, ILogger<DbInitializer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task DbInitializeAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Database initialization...");

            //_logger.LogInformation("Deleting the Database...");
            //await _db.Database.EnsureDeletedAsync().ConfigureAwait(false);
            //_logger.LogInformation("Delete the Database completed in {0} ms", timer.ElapsedMilliseconds);

            _logger.LogInformation("Database migration...");
            await _db.Database.MigrateAsync();
            _logger.LogInformation("Database migration completed in {0} ms", timer.ElapsedMilliseconds);

            if (await _db.Courses.AnyAsync())
                return;

            await InitializeCoursesAsync();
            await InitializeTeachresAsync();
            await InitializeGroupsAsync();
            await InitializeStudentsAsync();

            _logger.LogInformation("Database initialization completed in {0} s", timer.Elapsed.TotalSeconds);
        }

        private const int _COURSES_COUNT = 4;
        private Course[] _courses;
        private async Task InitializeCoursesAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Courses initialization...");

            _courses = Enumerable.Range(1, _COURSES_COUNT)
                .Select(i => new Course
                {
                    Name = $"Course {i}",
                    Description = $"Description course {i}"
                })
                .ToArray();

            await _db.Courses.AddRangeAsync(_courses);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Courses initialization completed in {0} ms", timer.ElapsedMilliseconds);
        }

        private const int _TEACHERS_COUNT = 5;
        private Teacher[] _teachers;
        private async Task InitializeTeachresAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Teachers initialization...");

            _teachers = Enumerable.Range(1, _TEACHERS_COUNT)
                .Select(i => new Teacher
                {
                    Name = $"Teacher {i}",
                    Surname = $"Teacher_surname {i}"
                })
                .ToArray();

            await _db.Teachers.AddRangeAsync(_teachers);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Teachers initialization completed in {0} ms", timer.ElapsedMilliseconds);
        }

        private const int _GROUPS_COUNT = 7;
        private Group[] _groups;
        private async Task InitializeGroupsAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Groups initialization...");

            var rnd = new Random();
            _groups = Enumerable.Range(1, _GROUPS_COUNT)
                .Select(i => new Group
                {
                    Name = $"Group {i}",
                    Course = rnd.NextItem(_courses),
                    Teacher = rnd.NextItem(_teachers)
                })
                .ToArray();

            await _db.Groups.AddRangeAsync(_groups);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Groups initialization completed in {0} ms", timer.ElapsedMilliseconds);
        }

        private const int _STUDENTS_COUNT = 200;
        private Student[] _students;
        private async Task InitializeStudentsAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Students initialization...");

            var rnd = new Random();
            _students = Enumerable.Range(1, _STUDENTS_COUNT)
                .Select(i => new Student
                {
                    Name = $"Student {i}",
                    Surname = $"Student_surname {i}",
                    Group = rnd.NextItem(_groups)
                })
                .ToArray();

            await _db.Students.AddRangeAsync(_students);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Students initialization completed in {0} ms", timer.ElapsedMilliseconds);
        }
    }
}
