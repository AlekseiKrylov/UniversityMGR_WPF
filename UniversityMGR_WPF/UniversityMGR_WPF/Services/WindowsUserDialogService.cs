using Microsoft.Win32;
using System;
using System.Windows;
using Task10;
using Task10.Views.Windows;
using UniversityMGR_WPF.Models;
using UniversityMGR_WPF.Services.Interfaces;
using UniversityMGR_WPF.ViewModels.UserDialog;

namespace UniversityMGR_WPF.Services
{
    internal class WindowsUserDialogService : IUserDialogService
    {
        public bool AddEdit(object item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return item switch
            {
                Course course => AddEditCourse(course),
                Group group => AddEditGroup(group),
                Student student => AddEditStudent(student),
                Teacher teacher => AddEditTeacher(teacher),
                _ => throw new NotSupportedException($"Editing object of type {item.GetType().Name} not supported"),
            };
        }

        public void ShowInformation(string message, string caption) =>
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);

        public void ShowWarning(string message, string caption) =>
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

        public void ShowError(string message, string caption) =>
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);

        public bool Confirm(string message, string caption, bool exclamation = false) =>
            MessageBox.Show(message, caption, MessageBoxButton.YesNo,
                exclamation ? MessageBoxImage.Exclamation : MessageBoxImage.Question) == MessageBoxResult.Yes;

        public bool OpenFile(string title, out string? selectedFile, string filter = "All files (*.*)|*.*")
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };

            if (dialog.ShowDialog() != true)
            {
                selectedFile = null;
                return false;
            }

            selectedFile = dialog.FileName;
            return true;
        }

        public bool SaveFile(string title, out string? filePath, string? fileName = null, string filter = "All files (*.*)|*.*")
        {
            filePath = null;

            var dialog = new SaveFileDialog
            {
                Title = title,
                Filter = filter
            };

            if (!string.IsNullOrWhiteSpace(fileName))
                dialog.FileName = fileName;

            if (dialog.ShowDialog() != true)
                return false;

            filePath = dialog.FileName;
            return true;
        }

        private static bool AddEditCourse(Course course)
        {
            string title = "Create new course";

            if (course.Id > 0)
                title = $"Edit course: {course.Name}";

            var dialog = new CourseEditorWindow
            {
                Title = title,
                Owner = App.ActiveWindow
            };

            var viewModel = (CourseEditorViewModel)dialog.DataContext;
            viewModel.Name = course.Name;
            viewModel.Description = course.Description;

            if (dialog.ShowDialog() != true)
                return false;

            course.Name = viewModel.Name;
            course.Description = viewModel.Description;

            return true;
        }

        private static bool AddEditGroup(Group group)
        {
            string title = "Create new group";

            if (group.Id > 0)
                title = $"Edit group: {group.Name}";

            var dialog = new GroupEditorWindow
            {
                Title = title,
                Owner = App.ActiveWindow
            };

            var viewModel = (GroupEditorViewModel)dialog.DataContext;
            viewModel.CourseName = group.Course.Name;
            viewModel.GroupName = group.Name;
            viewModel.Teacher = group.Teacher;

            if (dialog.ShowDialog() != true)
                return false;

            group.Name = viewModel.GroupName;
            group.Teacher = viewModel.Teacher;

            return true;
        }

        private static bool AddEditStudent(Student student)
        {
            string title = "Create new student";

            if (student.Id > 0)
                title = $"Edit student: {student.FullName}";

            var dialog = new StudentEditorWindow
            {
                Title = title,
                Owner = App.ActiveWindow
            };

            var viewModel = (StudentEditorViewModel)dialog.DataContext;
            viewModel.Name = student.Name;
            viewModel.Surname = student.Surname;
            viewModel.Group = student.Group;

            if (dialog.ShowDialog() != true)
                return false;

            student.Name = viewModel.Name;
            student.Surname = viewModel.Surname;
            student.Group = viewModel.Group;

            return true;
        }

        private static bool AddEditTeacher(Teacher teacher)
        {
            string title = "Create new teacher";

            if (teacher.Id > 0)
                title = $"Edit teacher: {teacher.FullName}";

            var dialog = new TeacherEditorWindow
            {
                Title = title,
                Owner = App.ActiveWindow
            };

            var viewModel = (TeacherEditorViewModel)dialog.DataContext;
            viewModel.Name = teacher.Name;
            viewModel.Surname = teacher.Surname;

            if (dialog.ShowDialog() != true)
                return false;

            teacher.Name = viewModel.Name;
            teacher.Surname = viewModel.Surname;

            return true;
        }
    }
}
