using Microsoft.Extensions.DependencyInjection;

namespace Task10.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowVM => App.AppHost!.Services.GetRequiredService<MainWindowViewModel>();
        public CoursesViewModel CoursesVM => App.AppHost!.Services.GetRequiredService<CoursesViewModel>();
        public GroupsViewModel GroupsVM => App.AppHost!.Services.GetRequiredService<GroupsViewModel>();
        public StudentsViewModel StudentsVM => App.AppHost!.Services.GetRequiredService<StudentsViewModel>();
        public TeachersViewModel TeachersVM => App.AppHost!.Services.GetRequiredService<TeachersViewModel>();
        public CourseEditorViewModel CourseEditorVM => App.AppHost!.Services.GetRequiredService<CourseEditorViewModel>();
        public GroupEditorViewModel GroupEditorVM => App.AppHost!.Services.GetRequiredService<GroupEditorViewModel>();
        public StudentEditorViewModel StudentEditorVM => App.AppHost!.Services.GetRequiredService<StudentEditorViewModel>();
        public TeacherEditorViewModel TeacherEditorVM => App.AppHost!.Services.GetRequiredService<TeacherEditorViewModel>();
    }
}
