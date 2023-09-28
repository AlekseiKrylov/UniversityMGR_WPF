using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Task10.Infrastructure.CustomAttribures;
using UniversityMGR_WPF.Models;
using UniversityMGR_WPF.Services.DbServices.Interfaces;
using UniversityMGR_WPF.ViewModels.Base;

namespace UniversityMGR_WPF.ViewModels.UserDialog
{
    internal class GroupEditorViewModel : ValidatableViewModelBase
    {
        private string _courseName;
        private string _groupName;
        private Teacher _teacher;
        private ObservableCollection<Teacher> _teachers;
        private readonly IDbService<Teacher> _dbTeacherService;

        public string CourseName
        {
            get => _courseName;
            set => SetProperty(ref _courseName, value);
        }

        [ValidateProperty]
        public string GroupName
        {
            get => _groupName;
            set => SetProperty(ref _groupName, value);
        }

        [ValidateProperty]
        public Teacher Teacher
        {
            get => _teacher;
            set => SetProperty(ref _teacher, value);
        }

        public ObservableCollection<Teacher> Teachers
        {
            get => _teachers;
            set => SetProperty(ref _teachers, value);
        }

        public GroupEditorViewModel(IDbService<Teacher> dbTheacherService)
        {
            _dbTeacherService = dbTheacherService;
            List<Teacher> teachers = _dbTeacherService.Items.ToList();
            _teachers = new ObservableCollection<Teacher>(teachers);
        }

        protected override string? Validate(string propertyName)
        {
            return propertyName switch
            {
                nameof(GroupName) => string.IsNullOrWhiteSpace(GroupName) ? "Name is required" : null,
                nameof(Teacher) => Teacher == null ? "Teacher is required" : null,
                _ => null
            };
        }
    }
}
