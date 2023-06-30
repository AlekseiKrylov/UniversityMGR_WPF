using System.Windows;
using Task10.Infrastructure.Commands.Base;

namespace Task10.Infrastructure.Commands
{
    internal class CloseDialogCommand : CommandBase
    {
        public bool? DialogResult { get; set; }

        protected override bool CanExecute(object? parameter) => (parameter is not null && parameter is Window);

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
