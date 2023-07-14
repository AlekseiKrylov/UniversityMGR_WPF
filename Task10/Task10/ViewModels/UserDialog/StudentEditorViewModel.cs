using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Task10.Models;
using Task10.Services.DbServices.Interfaces;
using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class StudentEditorViewModel : ViewModelBase
    {
        private Student _student;
        private ObservableCollection<Group> _groups;
        private readonly IDbService<Group> _dbGroupService;

        public Student Student
        {
            get => _student;
            set => SetProperty(ref _student, value);
        }

        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        public StudentEditorViewModel(IDbService<Group> dbGroupService)
        {
            _student = new();
            _dbGroupService = dbGroupService;
            List<Group> groups = _dbGroupService.Items.ToList();
            _groups = new ObservableCollection<Group>(groups);
        }
    }
}
