using System;
using System.Collections.Generic;
using System.IO;
using Task10.Infrastructure.FileGenerators;
using Task10.Infrastructure.FileParsers;
using Task10.Models;
using Task10.Services.Interfaces;

namespace Task10.Services
{
    class FileService : IFileService
    {
        public bool ExportToFile(object item, string path)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            
            var fileType = Path.GetExtension(path).ToUpper();

            return (item, fileType) switch
            {
                (Group group, ".PDF") => PDFGenerator.ExportGroupStudentsToPDF(group, path),
                (Group group, ".DOCX") => DOCXGenerator.ExportGroupDetailToDOCX(group, path),
                (List<Student> students, ".CSV") => CSVGenerator.ExportListOfStudentsToCSV(students, path),
                _ => throw new NotSupportedException($"Export object of type {item.GetType().Name} to {fileType} not supported"),
            };
        }

        public bool ImportFromFile(object item, string path, out object result, bool hasHeader = true)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            var fileType = Path.GetExtension(path).ToUpper();

            result = (item, fileType) switch
            {
                (Student, ".CSV") => CSVParser.ParseStudentsFromCSV(path, hasHeader),
                _ => throw new NotSupportedException($"Export object of type {item.GetType().Name} to {fileType} not supported"),
            };

            return true;
        }
    }
}
