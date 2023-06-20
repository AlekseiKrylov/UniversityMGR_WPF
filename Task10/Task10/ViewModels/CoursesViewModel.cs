using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    internal class CoursesViewModel : ViewModelBase
    {
        private readonly IDbService<Course> _dbCourseService;
        private ICommand _loadCoursesCommand;
        private ObservableCollection<Course> _courses;

        public ObservableCollection<Course> Courses
        {
            get { return _courses; }
            private set { SetProperty(ref _courses, value); }
        }

        public ICommand LoadCoursesCommand => _loadCoursesCommand
            ??= new RelayCommand(ExecuteLoadCoursesCommand);

        private void ExecuteLoadCoursesCommand()
        {
            List<Course> courses = _dbCourseService.Items.ToList();
            Courses = new ObservableCollection<Course>(courses);
        }

        public CoursesViewModel(IDbService<Course> dbCourseService)
        {
            _dbCourseService = dbCourseService;
        }
    }
}
