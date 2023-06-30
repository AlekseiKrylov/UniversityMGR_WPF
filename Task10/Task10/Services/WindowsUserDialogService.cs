using System;
using System.Linq;
using System.Windows;
using Task10.Models;
using Task10.Services.Interfaces;
using Task10.ViewModels;
using Task10.Views.Windows;

namespace Task10.Services
{
    internal class WindowsUserDialogService : IUserDialogService
    {
        private static Window activeWindow => Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

        public bool AddEdit(object item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            switch (item)
            {
                default: throw new NotSupportedException($"Editing object of type {item.GetType().Name} not supported");
                case Course course:
                    return AddEditCourse(course);
                case Group group:
                    return AddEditGroup(group);

            }
        }

        public void ShowInformation(string message, string caption) =>
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);

        public void ShowWarning(string message, string caption) =>
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

        public void ShowError(string message, string caption) =>
            MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Error);

        public bool Confirm(string message, string caption, bool exclamation = false) =>
            MessageBox.Show(message, caption, MessageBoxButton.YesNo,
                exclamation ? MessageBoxImage.Exclamation : MessageBoxImage.Question) == MessageBoxResult.Yes;

        private static bool AddEditCourse(Course course)
        {
            string title = "Create new course";

            if (course.Id > 0)
                title = $"Edit course: {course.Name}";

            Course tmpCourse = new()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description
            };

            var dialog = new CourseEditorWindow
            {
                Title = title,
                Owner = activeWindow
            };

            var viewModel = (CourseEditorViewModel)dialog.DataContext;
            viewModel.Course = tmpCourse;

            if (dialog.ShowDialog() != true)
                return false;

            course.Name = viewModel.Course.Name;
            course.Description = viewModel.Course.Description;

            return true;
        }

        private static bool AddEditGroup(Group group)
        {
            string title = "Create new group";

            if (group.Id > 0)
                title = $"Edit group: {group.Name}";

            Group tmpGroup = new()
            {
                Id = group.Id,
                Name = group.Name,
                Course = group.Course,
                Teacher = group.Teacher,
            };

            var dialog = new GroupEditorWindow
            {
                Title = title,
                Owner = activeWindow
            };

            var viewModel = (GroupEditorViewModel)dialog.DataContext;
            viewModel.Group = tmpGroup;

            if (dialog.ShowDialog() != true)
                return false;

            group.Name = viewModel.Group.Name;
            group.Course = viewModel.Group.Course;
            group.Teacher = viewModel.Group.Teacher;

            return true;
        }
    }
}
