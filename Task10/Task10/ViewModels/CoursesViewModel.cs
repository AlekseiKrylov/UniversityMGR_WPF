using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Task10.Infrastructure.Commands;
using Task10.Infrastructure.Commands.Base;
using Task10.Models;
using Task10.Services.Interfaces;
using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class CoursesViewModel : ViewModelBase
    {
        private readonly IDbService<Course> _dbCourseService;
        private ObservableCollection<Course> _courses;
        private Course _selectedCourse;

        public ObservableCollection<Course> Courses
        {
            get { return _courses; }
            private set { SetProperty(ref _courses, value); }
        }

        public Course SelectedCourse
        {
            get { return _selectedCourse; }
            set { SetProperty(ref _selectedCourse, value); }
        }

        #region LoadCoursesCommand
        private ICommand _loadCoursesCommand;

        public ICommand LoadCoursesCommand => _loadCoursesCommand
            ??= new RelayCommand(OnLoadCoursesCommandExecuted, CanLoadCoursesCommandExecute);

        private bool CanLoadCoursesCommandExecute() => true;

        private void OnLoadCoursesCommandExecuted()
        {
            List<Course> courses = _dbCourseService.Items.ToList();
            Courses = new ObservableCollection<Course>(courses);
        }
        #endregion

        #region SelectCourseCommand
        private ICommand _selectCourseCommand;

        public ICommand SelectCourseCommand => _selectCourseCommand
            ??= new RelayCommand<Course>(ExecuteSelectCourseCommand);

        private async void ExecuteSelectCourseCommand(Course selectedCourse)
        {
            ((CommandBase)SelectCourseCommand).Executable = false;

            SelectedCourse = await _dbCourseService.GetAsync(selectedCourse.Id) ;
            OnPropertyChanged(nameof(SelectedCourse));
            
            ((CommandBase)SelectCourseCommand).Executable = true;
        }
        #endregion

        #region CreateCommand

        private ICommand _createCourseCommand;
        public ICommand CreateCourseCommand => _createCourseCommand
            ??= new RelayCommand<Course>(ExecuteCreateCourseCommand);

        private void ExecuteCreateCourseCommand(Course course)
        {
        }


        #endregion

        #region DeleteCommand

        private ICommand _deleteCourseCommand;
        public ICommand DeleteCourseCommand => _deleteCourseCommand
            ??= new RelayCommand(ExecuteDeleteCourseCommand
                //,CanExecuteDeleteCourseCommand
                );

        //private bool CanExecuteDeleteCourseCommand(Course course)
        //{
        //    if (course is null)
        //        return false;
            
        //    return true;
        //}

        private void ExecuteDeleteCourseCommand(object p)
        {
            var course = (Course)p;
            _dbCourseService.RemoveAsync(course.Id);
        }

        #endregion


        public CoursesViewModel(IDbService<Course> dbCourseService)
        {
            _dbCourseService = dbCourseService;
        }
    }
}
