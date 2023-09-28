using Microsoft.Extensions.DependencyInjection;
using UniversityMGR_WPF.ViewModels.UserDialog;

namespace UniversityMGR_WPF.ViewModels
{
    internal class ViewModelLocator
    {
        public static MainWindowViewModel MainWindowVM => App.AppHost!.Services.GetRequiredService<MainWindowViewModel>();
        public static CoursesAndGroupsViewModel CoursesAndGroupsVM => App.AppHost!.Services.GetRequiredService<CoursesAndGroupsViewModel>();
        public static StudentsViewModel StudentsVM => App.AppHost!.Services.GetRequiredService<StudentsViewModel>();
        public static TeachersViewModel TeachersVM => App.AppHost!.Services.GetRequiredService<TeachersViewModel>();
        public static CourseEditorViewModel CourseEditorVM => App.AppHost!.Services.GetRequiredService<CourseEditorViewModel>();
        public static GroupEditorViewModel GroupEditorVM => App.AppHost!.Services.GetRequiredService<GroupEditorViewModel>();
        public static StudentEditorViewModel StudentEditorVM => App.AppHost!.Services.GetRequiredService<StudentEditorViewModel>();
        public static TeacherEditorViewModel TeacherEditorVM => App.AppHost!.Services.GetRequiredService<TeacherEditorViewModel>();
    }
}
