using System.Windows;
using UniversityMGR_WPF.Infrastructure.Commands.Base;

namespace UniversityMGR_WPF.Infrastructure.Commands
{
    internal class CloseWindowCommand : CommandBase
    {
        protected override bool CanExecute(object? parameter) => parameter is not null && parameter is Window;

        protected override void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
                return;

            var window = (Window)parameter!;
            window.Close();
        }
    }
}
