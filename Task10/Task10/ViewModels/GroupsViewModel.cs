using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Task10.Infrastructure.Commands;
using Task10.Models;
using Task10.Services.Interfaces;
using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class GroupsViewModel : ViewModelBase
    {
        private readonly IDbService<Group> _dbGroupService;
        private ObservableCollection<Group>? _groups;
        private readonly Student _placeholderStudent = new() { Name = "Loading..." };

        public ObservableCollection<Group>? Groups
        {
            get => _groups;
            private set => SetProperty(ref _groups, value);
        }

        public GroupsViewModel(IDbService<Group> dbGroupService) => _dbGroupService = dbGroupService;

        #region LoadGroupsWithStudentsCommand

        private RelayCommand _loadGroupWithStudentsCommand;
        public ICommand LoadGroupsWithStudentsCommand => _loadGroupWithStudentsCommand
            ??= new(OnLoadGroupsWithStudentsCommandExecuted);

        private void OnLoadGroupsWithStudentsCommandExecuted()
        {
            List<Group> groups = _dbGroupService.Items.Include(g => g.Students).Include(g => g.Teacher).ToList();
            Groups = new ObservableCollection<Group>(groups);
            OnPropertyChanged(nameof(Groups));
        }

        #endregion

        #region LoadGroupsWithoutStudentsCommand

        private RelayCommand _loadGroupWithoutStudentsCommand;
        public ICommand LoadGroupsWithoutStudentsCommand => _loadGroupWithoutStudentsCommand
            ??= new(OnLoadGroupsWithoutStudentsCommandExecuted);

        private void OnLoadGroupsWithoutStudentsCommandExecuted()
        {
            List<Group> groups = _dbGroupService.Items.ToList();

            foreach (Group group in groups)
                group.Students.Add(_placeholderStudent);

            Groups = new ObservableCollection<Group>(groups);
            OnPropertyChanged(nameof(Groups));
        }

        #endregion

        #region LoadStudentsCommand

        private RelayCommand _loadStudentsCommand;
        public ICommand LoadStudentsCommand => _loadStudentsCommand
            ??= new(OnLoadStudentsCommandExecuted, CanLoadStudentsCommandExecute);

        private bool CanLoadStudentsCommandExecute(object? p) => !(p is null || p is not Group);

        private async void OnLoadStudentsCommandExecuted(object? p)
        {
            var expandingGroup = (Group)p!;

            await _dbGroupService.GetDetailAsync(expandingGroup.Id).ConfigureAwait(false);

            if (expandingGroup.Students.Contains(_placeholderStudent))
                expandingGroup.Students.Remove(_placeholderStudent);
        }

        #endregion

    }
}
