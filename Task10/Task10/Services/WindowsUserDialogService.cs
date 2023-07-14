using Microsoft.Win32;
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
        private static Window _ActiveWindow => Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

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
            MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Error);

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

            Course tmpCourse = new()
            {
                Name = course.Name,
                Description = course.Description
            };

            var dialog = new CourseEditorWindow
            {
                Title = title,
                Owner = _ActiveWindow
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
                Name = group.Name,
                Course = group.Course,
                Teacher = group.Teacher,
            };

            var dialog = new GroupEditorWindow
            {
                Title = title,
                Owner = _ActiveWindow
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

        private static bool AddEditStudent(Student student)
        {
            string title = "Create new student";

            if (student.Id > 0)
                title = $"Edit student: {student.FullName}";

            Student tmpStudent = new()
            {
                Name = student.Name,
                Surname = student.Surname,
                Group = student.Group
            };

            var dialog = new StudentEditorWindow
            {
                Title = title,
                Owner = _ActiveWindow
            };

            var viewModel = (StudentEditorViewModel)dialog.DataContext;
            viewModel.Student = tmpStudent;

            if (dialog.ShowDialog() != true)
                return false;

            student.Name = viewModel.Student.Name;
            student.Surname = viewModel.Student.Surname;
            student.Group = viewModel.Student.Group;

            return true;
        }

        private static bool AddEditTeacher(Teacher teacher)
        {
            string title = "Create new teacher";

            if (teacher.Id > 0)
                title = $"Edit teacher: {teacher.FullName}";

            Teacher tmpTeacher = new()
            {
                Name = teacher.Name,
                Surname = teacher.Surname,
            };

            var dialog = new TeacherEditorWindow
            {
                Title = title,
                Owner = _ActiveWindow
            };

            var viewModel = (TeacherEditorViewModel)dialog.DataContext;
            viewModel.Teacher = tmpTeacher;

            if (dialog.ShowDialog() != true)
                return false;

            teacher.Name = viewModel.Teacher.Name;
            teacher.Surname = viewModel.Teacher.Surname;

            return true;
        }
    }
}
