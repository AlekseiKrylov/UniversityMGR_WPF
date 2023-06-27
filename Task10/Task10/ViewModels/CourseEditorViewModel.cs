using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Task10.Infrastructure.Commands;
using Task10.Models;
using Task10.Services.Interfaces;
using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class CourseEditorViewModel : ViewModelBase
    {
        private readonly IDbService<Course> _dbCourseService;
        private Course _course;
        public Course Course 
        {
            get => _course;
            set => SetProperty(ref _course, value);
        }

        private ICommand _saveCommand;

        public ICommand SaveCommand => _saveCommand
            ??= new RelayCommand(ExecuteSaveCommand);

        private void ExecuteSaveCommand(object obj)
        {
            if(Course.Id > 0)
                _dbCourseService.UpdateAsync(Course);

            _dbCourseService.AddAsync(Course);
        }

        public CourseEditorViewModel(IDbService<Course> dbCourseService)
        {
            _dbCourseService = dbCourseService;
        }
    }
}
