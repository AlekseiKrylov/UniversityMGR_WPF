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
    internal class TeachersViewModel : ViewModelBase
    {
        private readonly IDbService<Teacher> _dbTeacherService;
        private readonly IUserDialogService _userDialogService;
        private ObservableCollection<Teacher>? _teachers;
        private ObservableCollection<Group>? _groups;
        private Teacher? _selectedTeacher;

        public ObservableCollection<Teacher>? Teachers
        {
            get => _teachers;
            private set => SetProperty(ref _teachers, value);
        }

        public ObservableCollection<Group>? Groups
        {
            get => _groups;
            private set => SetProperty(ref _groups, value);
        }

        public Teacher? SelectedTeacher
        {
            get => _selectedTeacher;
            set => SetProperty(ref _selectedTeacher, value);
        }

        public TeachersViewModel(IUserDialogService userDialogService,
            IDbService<Teacher> dbTeacherService)
        {
            _dbTeacherService = dbTeacherService;
            _userDialogService = userDialogService;
        }

        #region COMMANDS

        #region LoadTeachersCommand

        private RelayCommand? _loadTeachersCommand;

        public ICommand LoadTeachersCommand =>
            _loadTeachersCommand ??= new(OnLoadTeachersCommandExecuted);

        private void OnLoadTeachersCommandExecuted() => UpdateTeachersList();

        #endregion

        #region SelectTeacherCommand

        private RelayCommand? _selectTeacherCommand;

        public ICommand SelectTeacherCommand =>
            _selectTeacherCommand ??= new(OnSelectTeacherCommandExecuted, CanSelectTeacherCommandExecute);

        private bool CanSelectTeacherCommandExecute(object? p) => !(p is null || p is not Teacher);

        async void OnSelectTeacherCommandExecuted(object? p)
        {
            var selectedTeacher = (Teacher)p!;
            await UpdateSelectedTeacher(selectedTeacher.Id);
        }

        #endregion

        #region CreateTeacherCommand

        private RelayCommand createTeacherCommand;

        public ICommand CreateTeacherCommand => createTeacherCommand
            ??= new RelayCommand(OnCreateTeacherCommandExecuted);

        private async void OnCreateTeacherCommandExecuted()
        {
            var newTeacher = new Teacher();

            if (_userDialogService.AddEdit(newTeacher))
                await _dbTeacherService.AddAsync(newTeacher);

            UpdateTeachersList();
            await UpdateSelectedTeacher(newTeacher.Id);
            SelectedTeacher = newTeacher;
        }

        #endregion

        #region EditTeacherCommand

        private RelayCommand editTeacherCommand;

        public ICommand EditTeacherCommand => editTeacherCommand
            ??= new RelayCommand(OnEditTeacherCommandExecuted, CanEditTeacherCommandExecute);

        private bool CanEditTeacherCommandExecute(object? p) => !(p is null || p is not Teacher);

        private async void OnEditTeacherCommandExecuted(object? p)
        {
            if (_userDialogService.AddEdit(p!))
                await _dbTeacherService.UpdateAsync((Teacher)p!);

            UpdateTeachersList();
            await UpdateSelectedTeacher(((Teacher)p!).Id);
            OnPropertyChanged(nameof(SelectedTeacher));
        }

        #endregion

        #region DeleteTeacherCommand

        private RelayCommand deleteTeacherCommand;

        public ICommand DeleteTeacherCommand => deleteTeacherCommand
            ??= new RelayCommand(OnDeleteTeacherCommandExecute, CanDeleteTeacherCommandExecute);

        private bool CanDeleteTeacherCommandExecute(object? p) => !(p is null || p is not Teacher);

        private async void OnDeleteTeacherCommandExecute(object? p)
        {
            var teacher = (Teacher)p!;
            string confirmMessage = $"Are you sure you want to delete teacher '{teacher.FullName}'?";
            string caption = "Delete teacher";

            if (!_userDialogService.Confirm(confirmMessage, caption))
                return;

            await _dbTeacherService.RemoveAsync(teacher.Id);
            UpdateTeachersList();
            SelectedTeacher = null;
        }

        #endregion

        #endregion

        private void UpdateTeachersList()
        {
            List<Teacher> teachers = _dbTeacherService.Items.ToList();
            Teachers = new ObservableCollection<Teacher>(teachers);
        }

        private async Task UpdateSelectedTeacher(int id)
        {
            SelectedTeacher = await _dbTeacherService.GetDetailAsync(id);
            Groups = new ObservableCollection<Group>(SelectedTeacher.Groups);
        }
    }
}
