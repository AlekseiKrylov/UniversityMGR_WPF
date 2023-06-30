using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Task10.Infrastructure.Commands;
using Task10.Models;
using Task10.Services.Interfaces;
using Task10.ViewModels.Base;
using Task10.Views.Windows;

namespace Task10.ViewModels
{
    internal class CoursesViewModel : ViewModelBase
    {
        private readonly IDbService<Course> _dbCourseService;
        private readonly IDbService<Group> _dbGroupService;
        private readonly IUserDialogService _userDialogService;
        private ObservableCollection<Course>? _courses;
        private Course? _selectedCourse;
        private Group? _selectedGroup;

        public ObservableCollection<Course>? Courses
        {
            get { return _courses; }
            private set { SetProperty(ref _courses, value); }
        }

        public Course? SelectedCourse
        {
            get { return _selectedCourse; }
            set { SetProperty(ref _selectedCourse, value); }
        }

        public Group? SelectedGroup
        {
            get { return _selectedGroup; }
            set { SetProperty(ref _selectedGroup, value); }
        }

        #region Curse commands

        #region LoadCoursesCommand

        private RelayCommand? _loadCoursesCommand;

        public ICommand LoadCoursesCommand => _loadCoursesCommand
            ??= new(OnLoadCoursesCommandExecuted);

        private void OnLoadCoursesCommandExecuted()
        {
            List<Course> courses = _dbCourseService.Items.ToList();
            Courses = new ObservableCollection<Course>(courses);
        }

        #endregion

        #region SelectCourseCommand

        private RelayCommand? _selectCourseCommand;

        public ICommand SelectCourseCommand => _selectCourseCommand
            ??= new(OnSelectCourseCommandExecuted, p => p is not null);

        private async void OnSelectCourseCommandExecuted(object? p)
        {
            SelectedGroup = null;
            var selectedCourse = (Course)p!;
            SelectedCourse = await _dbCourseService.GetAsync(selectedCourse.Id);
            OnPropertyChanged(nameof(SelectedCourse));
        }

        #endregion

        #region CreateCourseCommand

        private ICommand? _createCourseCommand;
        public ICommand CreateCourseCommand => _createCourseCommand
            ??= new RelayCommand(OnCreateCourseCommandExecuted);

        private async void OnCreateCourseCommandExecuted(object? p)
        {
            var newCourse = new Course();
            
            if (_userDialogService.AddEdit(newCourse))
                await _dbCourseService.AddAsync(newCourse);
        }

        #endregion

        #region EditCourseCommand

        private ICommand? _editCourseCommand;
        public ICommand EditCourseCommand => _editCourseCommand
            ??= new RelayCommand(OnEditCourseCommandExecuted, CanEditCourseCommandExecute);

        private bool CanEditCourseCommandExecute(object? p) => !(p is null || p is not Course);

        private async void OnEditCourseCommandExecuted(object? p)
        {
            if (_userDialogService.AddEdit(p!))
            {
                await _dbCourseService.UpdateAsync((Course)p!);
                _userDialogService.ShowInformation("Course edited", "Information");
            }
            else
                _userDialogService.ShowWarning("Editing rejection", "Warning");
        }

        #endregion
        
        #region DeleteCourseCommand

        private ICommand? _deleteCourseCommand;
        public ICommand DeleteCourseCommand => _deleteCourseCommand
            ??= new RelayCommand(OnDeleteCourseCommandExecuted, CanDeleteCourseCommandExecute);

        private bool CanDeleteCourseCommandExecute(object? p) => !(p is null || p is not Course);

        private async void OnDeleteCourseCommandExecuted(object? p)
        {
            var course = (Course)p!;
            string confirmMessage = $"Are you sure you want to delete course '{course.Name}'?";
            string warningMessage = "You cannot delete a course with groups";
            string caption = "Delete course";

            if (course.Groups!.Count > 0)
            {
                _userDialogService.ShowWarning(warningMessage, caption);
                return;
            }
            
            if (!_userDialogService.Confirm(confirmMessage, caption))
                return;
            
            await _dbCourseService.RemoveAsync(course.Id);
            SelectedCourse = null;
        }

        #endregion
        
        #endregion

        #region Group commands

        #region SelectGroupCommand

        private RelayCommand? _selectGroupCommand;

        public ICommand SelectGroupCommand => _selectGroupCommand
            ??= new(OnSelectGroupCommandExecuted, p => p is not null);

        private async void OnSelectGroupCommandExecuted(object? p)
        {
            var selectedGroup = (Group)p!;
            SelectedGroup = await _dbGroupService.GetAsync(selectedGroup.Id);
            OnPropertyChanged(nameof(SelectedGroup));
        }

        #endregion

        #region CreateGroupCommand

        private ICommand? _createGroupCommand;
        public ICommand CreateGroupCommand => _createGroupCommand
            ??= new RelayCommand(ExecuteCreateGroupCommand, CanExecuteCreateGroupCommand);

        private bool CanExecuteCreateGroupCommand(object? p) => !(p is null || p is not Course);

        private async void ExecuteCreateGroupCommand(object? p)
        {
            var course = (Course)p!;
            var newGroup = new Group();
            newGroup.Course = course;

            if (_userDialogService.AddEdit(newGroup))
                await _dbGroupService.AddAsync(newGroup);
        }

        #endregion

        #region EditGroupCommand

        private ICommand? _editGroupCommand;
        public ICommand EditGroupCommand => _editGroupCommand
            ??= new RelayCommand(OnEditGroupCommandExecuted, CanEditGroupCommandExecute);

        private bool CanEditGroupCommandExecute(object? p) => !(p is null || p is not Group);

        private async void OnEditGroupCommandExecuted(object? p)
        {
            if (_userDialogService.AddEdit(p!))
                await _dbGroupService.UpdateAsync((Group)p!);
        }

        #endregion

        #region DeleteGroupCommand

        private ICommand? _deleteGroupCommand;
        public ICommand DeleteGroupCommand => _deleteGroupCommand
            ??= new RelayCommand(OnDeleteGroupCommandExecuted, CanDeleteGroupCommandExecute);

        private bool CanDeleteGroupCommandExecute(object? p) => !(p is null || p is not Group);

        private async void OnDeleteGroupCommandExecuted(object? p)
        {
            var group = (Group)p!;
            string confirmMessage = $"Are you sure you want to delete Group '{group.Name}'?";
            string warningMessage = "You cannot delete a Group with students";
            string caption = "Delete Group";
            
            if (group.Students!.Count > 0)
            {
                _userDialogService.ShowWarning(warningMessage, caption);
                return;
            }

            if (!_userDialogService.Confirm(confirmMessage, caption))
                return;

            await _dbGroupService.RemoveAsync(group.Id);
        }

        #endregion

        #region ImportStudentsFromFileCommand !!!TO DO!!!

        #endregion

        #region ExportStudentsToFileCommand !!!TO DO!!!

        #endregion

        #region ExportGroupDetails !!!TO DO!!!

        #endregion

        #endregion

        public CoursesViewModel(IUserDialogService userDialogService,
            IDbService<Course> dbCourseService,
            IDbService<Group> dbGroupService)
        {
            _dbCourseService = dbCourseService;
            _dbGroupService = dbGroupService;
            _userDialogService = userDialogService;
        }
    }
}
