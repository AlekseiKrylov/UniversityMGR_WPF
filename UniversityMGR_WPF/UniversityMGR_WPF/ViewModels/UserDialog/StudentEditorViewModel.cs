﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UniversityMGR_WPF.Infrastructure.CustomAttribures;
using UniversityMGR_WPF.Models;
using UniversityMGR_WPF.Services.DbServices.Interfaces;
using UniversityMGR_WPF.ViewModels.Base;

namespace UniversityMGR_WPF.ViewModels.UserDialog
{
    internal class StudentEditorViewModel : ValidatableViewModelBase
    {
        private string _name;
        private string? _surname;
        private Group? _group;
        private ObservableCollection<Group> _groups;
        private readonly Group _emptyGroup = new() { Name = "No Group" };
        private readonly IDbService<Group> _dbGroupService;

        [ValidateProperty]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string? Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        public Group? Group
        {
            get => _group;
            set
            {
                if (value != null && value.Name == _emptyGroup.Name)
                    value = null;
                SetProperty(ref _group, value);
            }
        }

        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        public StudentEditorViewModel(IDbService<Group> dbGroupService)
        {
            _dbGroupService = dbGroupService;
            List<Group> groups = _dbGroupService.Items.ToList();
            groups.Insert(0, _emptyGroup);
            _groups = new ObservableCollection<Group>(groups);
        }

        protected override string? Validate(string propertyName)
        {
            return propertyName switch
            {
                nameof(Name) => string.IsNullOrWhiteSpace(Name) ? "Name is required" : null,
                _ => null
            };
        }
    }
}
