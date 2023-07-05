using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Task10.Models;
using Task10.Services.Interfaces;
using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class GroupEditorViewModel : ViewModelBase
    {
        private Group _group;
        private ObservableCollection<Teacher> _teachers;
        private IDbService<Teacher> _dbTeacherService;

        public Group Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        public ObservableCollection<Teacher> Teachers
        {
            get => _teachers;
            set => SetProperty(ref _teachers, value);
        }

        public GroupEditorViewModel(IDbService<Teacher> dbTheacherService)
        {
            _group = new();
            _dbTeacherService = dbTheacherService;
            List<Teacher> teachers = _dbTeacherService.Items.ToList();
            _teachers = new ObservableCollection<Teacher>(teachers);
        }
    }
}
