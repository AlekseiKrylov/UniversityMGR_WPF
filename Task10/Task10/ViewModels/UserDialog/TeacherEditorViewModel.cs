using Task10.Models;
using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class TeacherEditorViewModel : ViewModelBase
    {
		private Teacher _teacher;

		public Teacher Teacher
        {
            get => _teacher;
            set => SetProperty(ref _teacher, value);
        }

        public TeacherEditorViewModel() => _teacher = new();
    }
}
