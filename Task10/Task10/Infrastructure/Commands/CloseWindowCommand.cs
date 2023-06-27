using System;
using System.Windows;
using Task10.Infrastructure.Commands.Base;

namespace Task10.Infrastructure.Commands
{
    internal class CloseWindowCommand : CommandBase
    {
        public override bool CanExecute(object parameter) => parameter is Window;

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            var window = (Window) parameter;
            window.Close();
        }
    }

    internal class CloseDialogCommand : CommandBase
    {
        public bool? DialogResult { get; set; }
        
        public override bool CanExecute(object parameter) => parameter is Window;

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            var window = (Window)parameter;
            window.DialogResult = DialogResult;
            window.Close();
        }
    }
}
