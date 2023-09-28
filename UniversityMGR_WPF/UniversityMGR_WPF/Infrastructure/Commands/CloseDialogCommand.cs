using System.Windows;
using UniversityMGR_WPF.Infrastructure.Commands.Base;
using UniversityMGR_WPF.ViewModels.UserDialog.Interfaces;

namespace UniversityMGR_WPF.Infrastructure.Commands
{
    internal class CloseDialogCommand : CommandBase
    {
        public bool DialogResult { get; set; }

        protected override bool CanExecute(object? parameter)
        {
            if (parameter is null || parameter is not Window)
                return false;

            var window = (Window)parameter!;
            var viewModel = window.DataContext as IValidatable;

            if (DialogResult && window.DataContext as IValidatable is not null)
                return viewModel!.IsValid;

            return true;
        }

        protected override void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
                return;

            var window = (Window)parameter!;
            window.DialogResult = DialogResult;
            window.Close();
        }
    }
}
