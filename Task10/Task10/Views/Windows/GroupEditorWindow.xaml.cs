using System.ComponentModel;
using System.Windows;
using Task10.Models;

namespace Task10.Views.Windows
{
    public partial class GroupEditorWindow : Window
    {
        public static readonly DependencyProperty CourseNameProperty = DependencyProperty.Register(
            nameof(CourseName),
            typeof(string),
            typeof(GroupEditorWindow),
            new PropertyMetadata(default(string)));
        
        public string CourseName { get => (string)GetValue(CourseNameProperty); set => SetValue(CourseNameProperty, value); }

        public static readonly DependencyProperty CourseDescriptionProperty = DependencyProperty.Register(
            nameof(CourseDescription),
            typeof(string),
            typeof(GroupEditorWindow),
            new PropertyMetadata(default(string)));

        public string CourseDescription { get => (string)GetValue(CourseDescriptionProperty); set => SetValue(CourseDescriptionProperty, value); }

        public GroupEditorWindow() => InitializeComponent();
    }
}
