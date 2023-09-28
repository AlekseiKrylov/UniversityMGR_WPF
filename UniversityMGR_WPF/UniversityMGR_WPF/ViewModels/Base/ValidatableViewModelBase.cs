using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UniversityMGR_WPF.Infrastructure.CustomAttribures;
using UniversityMGR_WPF.ViewModels.UserDialog.Interfaces;

namespace UniversityMGR_WPF.ViewModels.Base
{
    internal abstract class ValidatableViewModelBase : INotifyPropertyChanged, IDataErrorInfo, IValidatable
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly Dictionary<string, string?> _errors = new();

        public bool IsValid => !_errors.Values.Any(x => x != null);

        public string Error => this[string.Empty];

        public string this[string columnName] => _errors.ContainsKey(columnName) ? _errors[columnName] : null;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                if (ShouldValidateProperty(propertyName))
                    ValidateProperty(propertyName);
                return false;
            }

            field = value;

            if (ShouldValidateProperty(propertyName))
                ValidateProperty(propertyName);

            OnPropertyChanged(propertyName);
            return true;
        }

        protected void ValidateProperty(string propertyName)
        {
            var error = Validate(propertyName);

            if (!string.IsNullOrEmpty(error))
                _errors[propertyName] = error;
            else
                _errors.Remove(propertyName);

            OnPropertyChanged(nameof(IsValid));
        }

        protected virtual bool ShouldValidateProperty(string propertyName)
        {
            var propertyInfo = GetType().GetProperty(propertyName);
            return propertyInfo?.GetCustomAttributes(typeof(ValidatePropertyAttribute), true).Any() == true;
        }

        protected abstract string? Validate(string propertyName);
    }
}