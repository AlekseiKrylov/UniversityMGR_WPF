using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Task10.Models;

namespace Task10.ViewModels
{
    public static class TreeViewItemBehavior
    {
        public static readonly DependencyProperty LoadStudentsCommandProperty =
            DependencyProperty.RegisterAttached(
                "LoadStudentsCommand",
                typeof(ICommand),
                typeof(TreeViewItemBehavior),
                new FrameworkPropertyMetadata(null, OnLoadStudentsCommandChanged));

        public static ICommand GetLoadStudentsCommand(TreeViewItem treeViewItem)
        {
            return (ICommand)treeViewItem.GetValue(LoadStudentsCommandProperty);
        }

        public static void SetLoadStudentsCommand(TreeViewItem treeViewItem, ICommand value)
        {
            treeViewItem.SetValue(LoadStudentsCommandProperty, value);
        }

        private static void OnLoadStudentsCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeViewItem treeViewItem)
                treeViewItem.Expanded += TreeViewItem_Expanded;
        }

        private static void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem treeViewItem && GetLoadStudentsCommand(treeViewItem) is ICommand command)
                if (treeViewItem.DataContext is Group group)
                    command.Execute(group);
        }
    }
}
