namespace Task10.Services.Interfaces
{
    internal interface IUserDialogService
    {
        bool AddEdit(object item);

        void ShowInformation(string message, string caption);

        void ShowWarning(string message, string caption);

        void ShowError(string message, string caption);

        bool Confirm(string message, string caption, bool exclamation = false);

        bool OpenFile(string title, out string? filePath, string filter);

        bool SaveFile(string title, out string? filePath, string? fileName, string filter);
    }
}
