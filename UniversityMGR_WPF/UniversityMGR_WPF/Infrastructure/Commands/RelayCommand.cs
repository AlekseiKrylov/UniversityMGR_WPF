using System;
using Task10.Infrastructure.Commands.Base;

namespace Task10.Infrastructure.Commands
{
    internal class RelayCommand : CommandBase
    {
        private readonly Delegate? _execute;
        private readonly Delegate? _canExecute;

        public RelayCommand(Action<object?> execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<object?, bool>? canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool CanExecute(object? parameter)
        {
            if (!base.CanExecute(parameter))
                return false;

            return _canExecute switch
            {
                null => true,
                Func<bool> canExec => canExec(),
                Func<object?, bool> canExec => canExec(parameter),
                _ => throw new NotSupportedException($"Delegate of type {_canExecute.GetType()} is not supported by command")
            };
        }

        protected override void Execute(object? parameter)
        {
            switch (_execute)
            {
                default: throw new NotSupportedException($"Delegate of type {_execute.GetType()} is not supported by command");
                case null: throw new InvalidOperationException($"Delegate 'execute' is not passed");

                case Action execute:
                    execute();
                    break;
                case Action<object?> execute:
                    execute(parameter);
                    break;
            }
        }
    }
}
