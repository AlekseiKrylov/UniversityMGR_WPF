using System.Windows;
using Task10.Infrastructure.Commands.Base;

namespace Task10.Infrastructure.Commands
{
    internal class CloseAppCommand : CommandBase
    {
        public override void Execute(object? parameter) => Application.Current.Shutdown();
    }
}
