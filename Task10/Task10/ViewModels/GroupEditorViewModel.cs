using Task10.Models;
using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class GroupEditorViewModel : ViewModelBase
    {
        private Group _group = new Group();

        public Group Group
        {
            get { return _group; }
            set { SetProperty(ref _group, value); }
        }

        public GroupEditorViewModel()
        {
            //_group = (Group)p;
        }
    }
}
