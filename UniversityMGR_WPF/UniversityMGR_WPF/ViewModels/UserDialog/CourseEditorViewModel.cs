using Task10.Infrastructure.CustomAttribures;
using UniversityMGR_WPF.ViewModels.Base;

namespace UniversityMGR_WPF.ViewModels.UserDialog
{
    internal class CourseEditorViewModel : ValidatableViewModelBase
    {
        private string _name;
        private string? _description;

        [ValidateProperty]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
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
