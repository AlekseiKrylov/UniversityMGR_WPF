using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Task10.Infrastructure.Commands;
using Task10.Models;
using Task10.Services.DbServices.Interfaces;
using Task10.Services.Interfaces;
using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class CoursesAndGroupsViewModel : ViewModelBase
    {
        private readonly IDbService<Course> _dbCourseService;
        private readonly IDbService<Group> _dbGroupService;
        private readonly IDbService<Student> _dbStudentService;
        private readonly IFileService _fileService;
        private readonly IUserDialogService _userDialogService;
        private ObservableCollection<Course>? _courses;
        private ObservableCollection<Group>? _groups;
        private ObservableCollection<Student>? _students;
        private Course? _selectedCourse;
        private Group? _selectedGroup;

        #region PROPERTIES

        public ObservableCollection<Course>? Courses
        {
            get => _courses;
            private set => SetProperty(ref _courses, value);
        }

        public ObservableCollection<Group>? Groups
        {
            get => _groups;
            private set => SetProperty(ref _groups, value);
        }

        public ObservableCollection<Student>? Students
        {
            get => _students;
            private set => SetProperty(ref _students, value);
        }

        public Course? SelectedCourse
        {
            get => _selectedCourse;
            set => SetProperty(ref _selectedCourse, value);
        }

        public Group? SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                SetProperty(ref _selectedGroup, value);
                OnPropertyChanged(nameof(IsGroupSelected));
                OnPropertyChanged(nameof(StudentsGroupBoxHeader));
                OnPropertyChanged(nameof(Students));
            }
        }

        public bool IsGroupSelected => SelectedGroup != null;

        public string StudentsGroupBoxHeader => SelectedGroup != null ? $"Students ({SelectedGroup.Students.Count}):" : string.Empty;

        #endregion

        public CoursesAndGroupsViewModel(IUserDialogService userDialogService,
            IDbService<Course> dbCourseService,
            IDbService<Group> dbGroupService,
            IDbService<Student> dbStudentService,
            IFileService fileService)
        {
            _dbCourseService = dbCourseService;
            _dbGroupService = dbGroupService;
            _dbStudentService = dbStudentService;
            _fileService = fileService;
            _userDialogService = userDialogService;
        }

        #region COMMANDS

        #region Curse commands

        #region LoadCoursesCommand

        private RelayCommand? _loadCoursesCommand;

        public ICommand LoadCoursesCommand => _loadCoursesCommand
            ??= new(OnLoadCoursesCommandExecuted);

        private void OnLoadCoursesCommandExecuted() => UpdateCourseList();

        #endregion

        #region SelectCourseCommand

        private RelayCommand? _selectCourseCommand;

        public ICommand SelectCourseCommand => _selectCourseCommand
            ??= new(OnSelectCourseCommandExecuted, CanSelectCourseCommandExecute);

        private bool CanSelectCourseCommandExecute(object? p) => !(p is null || p is not Course);

        private async void OnSelectCourseCommandExecuted(object? p)
        {
            SelectedGroup = null;
            var selectedCourse = (Course)p!;
            await UpdateSelectedCourseAsync(selectedCourse.Id);
        }

        #endregion

        #region CreateCourseCommand

        private ICommand? _createCourseCommand;

        public ICommand CreateCourseCommand => _createCourseCommand
            ??= new RelayCommand(OnCreateCourseCommandExecuted);

        private async void OnCreateCourseCommandExecuted()
        {
            var newCourse = new Course();

            if (_userDialogService.AddEdit(newCourse))
                await _dbCourseService.AddAsync(newCourse);

            UpdateCourseList();
            SelectedCourse = newCourse;
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
                await _dbCourseService.UpdateAsync((Course)p!);

            UpdateCourseList();
            OnPropertyChanged(nameof(SelectedCourse));
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
            Course deleteCourse = await _dbCourseService.GetDetailAsync(course.Id);
            string confirmMessage = $"Are you sure you want to delete course '{deleteCourse.Name}'?";
            string warningMessage = "You cannot delete a course with groups";
            string caption = "Delete course";

            if (deleteCourse.Groups!.Count > 0)
            {
                _userDialogService.ShowWarning(warningMessage, caption);
                return;
            }

            if (!_userDialogService.Confirm(confirmMessage, caption))
                return;

            await _dbCourseService.RemoveAsync(deleteCourse.Id);
            UpdateCourseList();
            SelectedCourse = null;
        }

        #endregion

        #endregion

        #region Group commands

        #region SelectGroupCommand

        private RelayCommand? _selectGroupCommand;

        public ICommand SelectGroupCommand => _selectGroupCommand
            ??= new(OnSelectGroupCommandExecuted, CanSelectGroupCommandExecute);

        private bool CanSelectGroupCommandExecute(object? p) => !(p is null || p is not Group);

        private async void OnSelectGroupCommandExecuted(object? p)
        {
            var selectedGroup = (Group)p!;
            await UpdateSelectedGroupAsync(selectedGroup.Id);
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
            var newGroup = new Group { Course = course };

            if (_userDialogService.AddEdit(newGroup))
                await _dbGroupService.AddAsync(newGroup);

            await UpdateSelectedCourseAsync(course.Id);
            SelectedGroup = newGroup;
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

            await UpdateSelectedCourseAsync(((Group)p!).CourseId);
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
            Group deleteGroup = await _dbGroupService.GetDetailAsync(group.Id);
            string confirmMessage = $"Are you sure you want to delete Group '{deleteGroup.Name}'?";
            string warningMessage = "You cannot delete a Group with students";
            string caption = "Delete Group";

            if (deleteGroup.Students!.Count > 0)
            {
                _userDialogService.ShowWarning(warningMessage, caption);
                return;
            }

            if (!_userDialogService.Confirm(confirmMessage, caption))
                return;

            await _dbGroupService.RemoveAsync(deleteGroup.Id);

            await UpdateSelectedCourseAsync(deleteGroup.CourseId);
        }

        #endregion

        #region ExportStudentsToFileCommand

        private RelayCommand _exportStudentsCommand;

        public ICommand ExportStudentsToFileCommand => _exportStudentsCommand
            ??= new(OnExportStudentsToFileCommandExecuted, CanExportStudentsToFileCommandExecute);

        private bool CanExportStudentsToFileCommandExecute(object? p) => !(p is null || p is not Group || ((Group)p).Students.Count < 1);

        private void OnExportStudentsToFileCommandExecuted(object? p)
        {
            var group = (Group)p!;
            var students = group.Students.ToList();
            string title = $"Export students to File";
            string filter = "PDF files (*.pdf)|*.pdf|Word files (*.docx)|*.docx|CSV files (*.csv)|*.csv";
            string fileName = $"{group.Course.Name}_{group.Name}";
            string headerText = $"{group.Course.Name} - {group.Name}";

            if (!_userDialogService.SaveFile(title, out string? filePath, fileName, filter))
                return;

            try
            {
                if (!_fileService.ExportToFile(students, filePath!, headerText))
                    _userDialogService.ShowWarning("Exports failed", title);

                _userDialogService.ShowInformation($"Exported in {filePath}", title);

            }
            catch (Exception ex)
            {
                _userDialogService.ShowError($"Export error: {ex.Message}", title);
            }
        }

        #endregion

        #region ImportStudentsFromFileCommand

        private RelayCommand _importStudentsFromFileCommand;

        public ICommand ImportStudentsFromFileCommand => _importStudentsFromFileCommand
            ??= new(OnImportStudentsFromFileCommandExecuted, CanImportStudentsFromFileCommandExecute);

        private bool CanImportStudentsFromFileCommandExecute(object? p) => !(p is null || p is not Group);

        private async void OnImportStudentsFromFileCommandExecuted(object? p)
        {
            var group = (Group)p!;
            string title = $"Import students from File";
            string filter = "CSV files (*.csv)|*.csv";

            if (!_userDialogService.OpenFile(title, out var filePath, filter))
                return;

            if (!_fileService.ImportFromFile(new Student(), filePath!, out object result, true))
                _userDialogService.ShowError($"Exports failed", title);

            var newStudents = (List<Student>)result;
            foreach (var student in newStudents)
                student.Group = group;

            var oldStudents = group.Students.ToList();
            foreach (var student in oldStudents)
                student.Group = null;

            await _dbStudentService.UpdateRangeAsync(oldStudents);
            await _dbStudentService.AddRangeAsync(newStudents);
            await UpdateSelectedGroupAsync(group.Id);
            _userDialogService.ShowInformation($"Students imported", title);
        }

        #endregion

        #endregion

        #endregion

        private void UpdateCourseList()
        {
            List<Course> courses = _dbCourseService.Items.ToList();
            Courses = new ObservableCollection<Course>(courses);
        }

        private async Task UpdateSelectedCourseAsync(int id)
        {
            SelectedCourse = await _dbCourseService.GetDetailAsync(id);
            Groups = new ObservableCollection<Group>(SelectedCourse.Groups!);
        }

        private async Task UpdateSelectedGroupAsync(int id)
        {
            SelectedGroup = await _dbGroupService.GetDetailAsync(id);
            Students = new ObservableCollection<Student>(SelectedGroup.Students);
        }
    }
}
