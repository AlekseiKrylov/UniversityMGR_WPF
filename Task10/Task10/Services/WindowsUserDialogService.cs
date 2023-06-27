using System;
using System.Windows;
using Task10.Models;
using Task10.Services.Interfaces;
using Task10.Views.Windows;

namespace Task10.Services
{
    internal class WindowsUserDialogService : IUserDialogService
    {
        public bool Edit(object item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            switch (item)
            {
                default: throw new NotSupportedException($"Editing object of type {item.GetType().Name} not supported");
                case Course course:
                    return EditCourse(course);

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

        private static bool EditCourse(Course course)
        {
            var dialog = new GroupEditorWindow
            {
                CourseName = course.Name,
                CourseDescription = course.Description
            };

            if (dialog.ShowDialog() != true)
                return false;

            course.Name = dialog.CourseName;
            course.Description = dialog.CourseDescription;

            return true;
        }
    }
}
