namespace Task10.Services.Interfaces
{
    internal interface IUserDialogService
    {
        bool AddEdit(object item);

        void ShowInformation(string message, string caption);

        void ShowWarning(string message, string caption);

        void ShowError(string message, string caption);

        bool Confirm(string message, string caption, bool exclamation = false);
    }
}
