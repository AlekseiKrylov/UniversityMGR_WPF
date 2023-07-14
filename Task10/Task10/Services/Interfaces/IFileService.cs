namespace Task10.Services.Interfaces
{
    internal interface IFileService
    {
        bool ExportToFile(object item, string fileType, string path);
    }
}
