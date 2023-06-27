using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Task10.Infrastructure.Commands;
using Task10.Infrastructure.Commands.Base;
using Task10.Models;
using Task10.Services.Interfaces;
using Task10.ViewModels.Base;
using Task10.Views.Windows;

namespace Task10.ViewModels
{
    internal class CoursesViewModel : ViewModelBase
    {
        private readonly IDbService<Course> _dbCourseService;
        private readonly IUserDialogService _userDialogService;
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

        private bool CanLoadCoursesCommandExecute(object p) => true;

        private void OnLoadCoursesCommandExecuted(object p)
        {
            List<Course> courses = _dbCourseService.Items.ToList();
            Courses = new ObservableCollection<Course>(courses);
        }

        #endregion

        #region SelectCourseCommand

        private ICommand _selectCourseCommand;

        public ICommand SelectCourseCommand => _selectCourseCommand
            ??= new RelayCommand(ExecuteSelectCourseCommand);

        private async void ExecuteSelectCourseCommand(object p)
        {
            var selectedCourse = (Course)p;
            SelectedCourse = await _dbCourseService.GetAsync(selectedCourse.Id) ;
            OnPropertyChanged(nameof(SelectedCourse));
        }

        #endregion

        #region CreateCourseCommand

        private ICommand _createCourseCommand;
        public ICommand CreateCourseCommand => _createCourseCommand
            ??= new RelayCommand(ExecuteCreateCourseCommand);

        private void ExecuteCreateCourseCommand(object p)
        {
            CourseEditorViewModel viewModel = new(_dbCourseService);
            viewModel.Course = new();

            CourseEditorWindow editorWindow = new();
            editorWindow.DataContext = viewModel;
            editorWindow.Show();
        }


        #endregion

        #region OpenCourseEditorCommand

        private ICommand _openCourseEditorCommand;
        public ICommand OpenCourseEditorCommand => _openCourseEditorCommand
            ??= new RelayCommand(ExecuteOpenCourseEditorCommand, CanExecuteOpenCourseEditorCommand);

        private bool CanExecuteOpenCourseEditorCommand(object p) => !(p is null || p is not Course);

        private void ExecuteOpenCourseEditorCommand(object p)
        {
            CourseEditorViewModel viewModel = new(_dbCourseService);
            viewModel.Course = (Course)p;

            CourseEditorWindow editorWindow = new();
            editorWindow.DataContext = viewModel;
            editorWindow.Show();
        }


        #endregion

        #region EditGroupCommand

        private ICommand _editGroupCommand;
        public ICommand EditGroupCommand => _editGroupCommand
            ??= new RelayCommand(ExecuteEditGroupCommand, CanExecuteEditGroupCommand);

        private bool CanExecuteEditGroupCommand(object p) => !(p is null || p is not Group);

        private void ExecuteEditGroupCommand(object p)
        {
            var selectedGroup = (Group)p;

            GroupEditorViewModel groupEditorViewModel = new GroupEditorViewModel();

            groupEditorViewModel.Group = selectedGroup;

            var dialog = new GroupEditorWindow();
            //dialog.ShowDialog();

            if (dialog.ShowDialog() == true)
                MessageBox.Show("Button OK");
            else
                MessageBox.Show("Button cancel");
        }

        #endregion

        #region EditGroupCommand TEST!!!!

        private ICommand _editCourseCommand;
        public ICommand EditCourseCommand => _editCourseCommand
            ??= new RelayCommand(ExecuteEditCourseCommand, CanExecuteEditCourseCommand);

        private bool CanExecuteEditCourseCommand(object p) => !(p is null || p is not Course);

        private void ExecuteEditCourseCommand(object p)
        {
            if (_userDialogService.Edit(p))
            {
                _dbCourseService.UpdateAsync((Course)p);
                _userDialogService.ShowInformation("Course edited", "Information");
            }

            _userDialogService.ShowWarning("Editing rejection", "Warning");
        }

        #endregion

        #region CreateNewGroupCommand

        private ICommand _createNewGroupCommand;
        public ICommand CreateNewGroupCommand => _createNewGroupCommand
            ??= new RelayCommand(ExecuteCreateNewGroupCommand, CanExecuteCreateNewGroupCommand);

        private bool CanExecuteCreateNewGroupCommand(object p) => p is Course;

        private void ExecuteCreateNewGroupCommand(object p)
        {
            var course = (Course)p;

        }

        #endregion

        #region DeleteCourseCommand

        private ICommand _deleteCourseCommand;
        public ICommand DeleteCourseCommand => _deleteCourseCommand
            ??= new RelayCommand(ExecuteDeleteCourseCommand, CanExecuteDeleteCourseCommand);

        private bool CanExecuteDeleteCourseCommand(object p) => !(p is null || p is not Course);

        private void ExecuteDeleteCourseCommand(object p)
        {
            var course = (Course)p;
            _dbCourseService.RemoveAsync(course.Id);
        }

        #endregion


        public CoursesViewModel(IDbService<Course> dbCourseService, IUserDialogService userDialogService)
        {
            _dbCourseService = dbCourseService;
            _userDialogService = userDialogService;
        }
    }
}
