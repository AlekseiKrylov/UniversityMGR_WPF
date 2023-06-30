using Task10.Models;
using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class CourseEditorViewModel : ViewModelBase
    {
        private Course _course;

        public Course Course
        {
            get => _course;
            set => SetProperty(ref _course, value);
        }

        public CourseEditorViewModel() => _course = new();
    }
}
