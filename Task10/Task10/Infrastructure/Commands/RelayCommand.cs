﻿using System;
using Task10.Infrastructure.Commands.Base;

namespace Task10.Infrastructure.Commands
{
    internal class RelayCommand : CommandBase
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter)
        {
            if(!CanExecute(parameter))
                return;
            
            _execute(parameter);
        }
    }
}
