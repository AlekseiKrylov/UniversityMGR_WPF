namespace Task10.Services.Interfaces
{
    internal interface IFileService
    {
        bool ExportToFile(object item, string path, string headerText = "");
        
        bool ImportFromFile(object item, string path, out object result, bool hasHeader = true);
    }
}
