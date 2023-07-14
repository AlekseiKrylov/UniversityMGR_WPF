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
    class StudentsViewModel : ViewModelBase
    {
        private readonly IDbService<Student> _dbStudentService;
        private readonly IUserDialogService _userDialogService;
        private ObservableCollection<Student>? _students;
        private Student? _selectedStudent;

        public ObservableCollection<Student>? Students
        {
            get => _students;
            private set => SetProperty(ref _students, value);
        }

        public Student? SelectedStudent
        {
            get => _selectedStudent;
            set => SetProperty(ref _selectedStudent, value);
        }

        public StudentsViewModel(IUserDialogService userDialogService,
            IDbService<Student> dbStudentService)
        {
            _dbStudentService = dbStudentService;
            _userDialogService = userDialogService;
        }

        #region COMMANDS

        #region LoadStudentsCommand

        private RelayCommand? _loadStudentsCommand;

        public ICommand LoadStudentsCommand =>
            _loadStudentsCommand ??= new(OnLoadStudentsCommandExecuted);

        private void OnLoadStudentsCommandExecuted() => UpdateStudentsList();

        #endregion

        #region SelectStudentCommand

        private RelayCommand? _selectStudentCommand;

        public ICommand SelectStudentCommand =>
            _selectStudentCommand ??= new(OnSelectStudentCommandExecuted, CanSelectStudentCommandExecute);

        private bool CanSelectStudentCommandExecute(object? p) => !(p is null || p is not Student);

        async void OnSelectStudentCommandExecuted(object? p)
        {
            var selectedStudent = (Student)p!;
            await UpdateSelectedStudent(selectedStudent.Id);
        }

        #endregion

        #region CreateStudentCommand

        private RelayCommand createStudentCommand;

        public ICommand CreateStudentCommand => createStudentCommand
            ??= new RelayCommand(OnCreateStudentCommandExecuted);

        private async void OnCreateStudentCommandExecuted()
        {
            var newStudent = new Student();

            if (_userDialogService.AddEdit(newStudent))
                await _dbStudentService.AddAsync(newStudent);

            UpdateStudentsList();
            await UpdateSelectedStudent(newStudent.Id);
            SelectedStudent = newStudent;
        }

        #endregion

        #region EditStudentCommand

        private RelayCommand editStudentCommand;

        public ICommand EditStudentCommand => editStudentCommand
            ??= new RelayCommand(OnEditStudentCommandExecuted, CanEditStudentCommandExecute);

        private bool CanEditStudentCommandExecute(object? p) => !(p is null || p is not Student);

        private async void OnEditStudentCommandExecuted(object? p)
        {
            if (_userDialogService.AddEdit(p!))
                await _dbStudentService.UpdateAsync((Student)p!);

            UpdateStudentsList();
            await UpdateSelectedStudent(((Student)p!).Id);
            OnPropertyChanged(nameof(SelectedStudent));
        }

        #endregion

        #region DeleteStudentCommand

        private RelayCommand deleteStudentCommand;

        public ICommand DeleteStudentCommand => deleteStudentCommand
            ??= new RelayCommand(OnDeleteStudentCommandExecute, CanDeleteStudentCommandExecute);

        private bool CanDeleteStudentCommandExecute(object? p) => !(p is null || p is not Student);

        private async void OnDeleteStudentCommandExecute(object? p)
        {
            var student = (Student)p!;
            string confirmMessage = $"Are you sure you want to delete student '{student.FullName}'?";
            string caption = "Delete student";

            if (!_userDialogService.Confirm(confirmMessage, caption))
                return;

            await _dbStudentService.RemoveAsync(student.Id);
            UpdateStudentsList();
            SelectedStudent = null;
        }

        #endregion

        #endregion

        private void UpdateStudentsList()
        {
            List<Student> students = _dbStudentService.Items.ToList();
            Students = new ObservableCollection<Student>(students);
        }

        private async Task UpdateSelectedStudent(int id)
        {
            SelectedStudent = await _dbStudentService.GetDetailAsync(id);
        }
    }
}
