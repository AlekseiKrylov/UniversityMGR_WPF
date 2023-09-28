using System.Windows.Controls;

namespace UniversityMGR_WPF.Infrastructure.CustomControls
{
    internal class ScrollingDataGrid : DataGrid
    {
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            var grid = e.Source as DataGrid;

            if (grid.SelectedItem is not null)
            {
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.SelectedItem, null);
            }

            base.OnSelectionChanged(e);
        }
    }
}
