using UniversityMGR_WPF.Infrastructure.CustomAttribures;
using UniversityMGR_WPF.ViewModels.Base;

namespace UniversityMGR_WPF.ViewModels.UserDialog
{
    internal class TeacherEditorViewModel : ValidatableViewModelBase
    {
        private string _name;
        private string? _surname;

        [ValidateProperty]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string? Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        protected override string? Validate(string propertyName)
        {
            return propertyName switch
            {
                nameof(Name) => string.IsNullOrWhiteSpace(Name) ? "Name is required" : null,
                _ => null
            };
        }
    }
}
