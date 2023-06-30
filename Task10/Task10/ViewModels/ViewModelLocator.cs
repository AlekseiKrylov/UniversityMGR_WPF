using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task10.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowVM => App.AppHost!.Services.GetRequiredService<MainWindowViewModel>();
        public CoursesViewModel CoursesVM => App.AppHost!.Services.GetRequiredService<CoursesViewModel>();
        public CourseEditorViewModel CourseEditorVM => App.AppHost!.Services.GetRequiredService<CourseEditorViewModel>();
        public GroupEditorViewModel GroupEditorVM => App.AppHost!.Services.GetRequiredService<GroupEditorViewModel>();
    }
}
