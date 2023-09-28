using Task10.ViewModels.Base;

namespace Task10.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _title = "University Application";
        private string _status = "OK";

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
    }
}
