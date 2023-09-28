using System.Windows;
using UniversityMGR_WPF.Infrastructure.Commands.Base;

namespace UniversityMGR_WPF.Infrastructure.Commands
{
    internal class CloseAppCommand : CommandBase
    {
        protected override void Execute(object? parameter) => Application.Current.Shutdown();
    }
}
